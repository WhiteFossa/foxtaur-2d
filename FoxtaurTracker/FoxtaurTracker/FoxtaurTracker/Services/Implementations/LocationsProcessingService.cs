using Android.Content;
using FoxtaurTracker.Constants;
using FoxtaurTracker.Services.Abstract;
using GeolocatorPlugin;
using GeolocatorPlugin.Abstractions;
using LibAuxiliary.Helpers;
using LibWebClient.Models;
using LibWebClient.Models.DTOs;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;
using System.Collections.Concurrent;
using System.Timers;
using Application = Android.App.Application;
using Timer = System.Timers.Timer;

namespace FoxtaurTracker.Services.Implementations;

public class LocationsProcessingService : ILocationsProcessingService
{
    private readonly IWebClient _webClient;
    private readonly INotificationsService _notificationsService;
    
    /// <summary>
    /// Last get coordinates event time
    /// </summary>
    private DateTime? _lastFixTime;

    /// <summary>
    /// Last sent coordinates event time
    /// </summary>
    private DateTime? _lastSendTime;

    /// <summary>
    /// Amount of positions, already sent to server
    /// </summary>
    private int _positionsSent;
    
    /// <summary>
    /// Coordinates queue is being checked by this timer
    /// </summary>
    private Timer _locationsSendTimer;
    
    /// <summary>
    /// Notification is updated by this timer
    /// </summary>
    private Timer _notificationUpdateTimer;

    private ConcurrentQueue<HunterLocation> _locationsQueue = new ConcurrentQueue<HunterLocation>();

    public LocationsProcessingService
    (
        IWebClient webClient,
        INotificationsService notificationsService
    )
    {
        _webClient = webClient;
        _notificationsService = notificationsService;

        // Setting up locations send timer
        _locationsSendTimer = new Timer(TimeSpan.FromSeconds(GlobalConstants.LocationsSendInterval));
        _locationsSendTimer.Elapsed += OnLocationSendTimer;
        _locationsSendTimer.AutoReset = true;
        _locationsSendTimer.Enabled = true;
        
        // Setting up notification update timer
        _notificationUpdateTimer = new Timer(TimeSpan.FromSeconds(GlobalConstants.NotificationUpdateInterval));
        _notificationUpdateTimer.Elapsed += OnNotificationUpdateTimer;
        _notificationUpdateTimer.AutoReset = true;
        _notificationUpdateTimer.Enabled = true;
    }

    public async Task StartTrackingAsync()
    {
        // Do we have the permission?
        var isPermitted = await CheckForLocationAlwaysPermissionAsync();
        if (!isPermitted)
        {
            await App.PopupsService.ShowAlertAsync("Warning", "Please set location permission to Always.");
            
            // Requesting
            var locationPermission = await Permissions.RequestAsync<Permissions.LocationAlways>();
            if (locationPermission != PermissionStatus.Granted)
            {
                await App.PopupsService.ShowAlertAsync("Error", "You didn't set location permission to Always, tracking can't be started.");
                return;
            }
        }
        
        _lastFixTime = null;
        _lastSendTime = null;
        _positionsSent = 0;
        
        
        try
        {
            if (!await CrossGeolocator.Current.StartListeningAsync
                (
                    TimeSpan.FromSeconds(GlobalConstants.LocationFetchInterval),
                    GlobalConstants.LocationFetchMinimalDistance,
                    true,
                    new ListenerSettings
                    {
                        ActivityType = ActivityType.AutomotiveNavigation,
                        AllowBackgroundUpdates = true,
                        DeferLocationUpdates = false,
                        ListenForSignificantChanges = false,
                        PauseLocationUpdatesAutomatically = false
                    })
               )
            {
                // Failed to enable GPS
                await MainThread.InvokeOnMainThreadAsync(async () => await App.PopupsService.ShowAlertAsync("Error", "Failed to start GPS tracking."));
                return;
            }
        }
        catch (GeolocationException ex)
        {
            if (ex.Error == GeolocationError.Unauthorized)
            {
                await MainThread.InvokeOnMainThreadAsync(async () =>
                    await App.PopupsService.ShowAlertAsync("Error", "Please give this application location permission."));
                return;
            }

            if (ex.Error == GeolocationError.PositionUnavailable)
            {
                // Swallowing the error
            }
            else
            {
                throw;
            }
        }

#if ANDROID
        // Starting foreground service to avoid to be killed
        var intent = new Intent(Application.Context, typeof(TrackerForegroundService));
        Application.Context.StartForegroundService(intent);
#endif

        CrossGeolocator.Current.PositionChanged += OnPositionChanged;
        CrossGeolocator.Current.PositionError += OnPositionError;
    }

    private void OnPositionChanged(object sender, PositionEventArgs e)
    {
        Task.WaitAll(OnPositionChangedAsync(sender, e));
    }

    private async Task OnPositionChangedAsync(object sender, PositionEventArgs e)
    {
        _lastFixTime = DateTime.UtcNow;

        var hunterLocation = new HunterLocation
        (
            Guid.NewGuid(),
            e.Position.Timestamp.UtcDateTime,
            e.Position.Latitude.ToRadians(),
            e.Position.Longitude.ToRadians(),
            e.Position.Altitude.ToRadians()
        );
        
        _locationsQueue.Enqueue(hunterLocation);
    }
    
    private void OnPositionError(object sender, PositionErrorEventArgs e)
    {
        Task.WaitAll(OnPositionErrorAsync(sender, e));
    }
    
    private async Task OnPositionErrorAsync(object sender, PositionErrorEventArgs e)
    {
        if (e.Error == GeolocationError.Unauthorized)
        {
            await StopTrackingAsync();
            await MainThread.InvokeOnMainThreadAsync(async () => await App.PopupsService.ShowAlertAsync("Error", "Please give this application location permission."));
            return;
        }
        
        // Swallowing "Position unavailable" error 
    }

    public async Task StopTrackingAsync()
    {
#if ANDROID
        var intent = new Intent(Application.Context, typeof(TrackerForegroundService));
        Application.Context.StopService(intent);
#endif

        if (!await CrossGeolocator.Current.StopListeningAsync())
        {
            await MainThread.InvokeOnMainThreadAsync(async () => await App.PopupsService.ShowAlertAsync("Error", "Failed to stop GPS tracking."));
            return;
        }

        CrossGeolocator.Current.PositionChanged -= OnPositionChanged;
        CrossGeolocator.Current.PositionError -= OnPositionError;
    }

    public async Task<bool> CheckForLocationAlwaysPermissionAsync()
    {
        var status = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();

        return status == PermissionStatus.Granted;
    }

    private void OnLocationSendTimer(object sender, ElapsedEventArgs e)
    {
        if (!_locationsQueue.Any())
        {
            return;
        }
        
        var toSend = _locationsQueue
            .Take(GlobalConstants.MaxSendLocationsCount)
            .ToList();

        var request = new CreateHunterLocationsRequest(toSend
            .Select(hl => new HunterLocationDto()
                {
                    Id = hl.Id,
                    Timestamp = hl.Timestamp,
                    Lat = hl.Lat,
                    Lon = hl.Lon,
                    Alt = hl.Alt
                })
            .ToList());

        IReadOnlyCollection<Guid> createdLocationsIds = null;
        try
        {
            createdLocationsIds = _webClient.CreateHunterLocationsAsync(request).Result;
        }
        catch (Exception)
        {
            // Swallowing exception
            // TODO: Add logging
            return;
        }
        
        // Regenerating queue
        var newLocationsQueue = new ConcurrentQueue<HunterLocation>();

        while (_locationsQueue.Any())
        {
            _locationsQueue.TryDequeue(out var location);
            
            if (createdLocationsIds.Any(clid => clid == location.Id))
            {
                // Newly-created location
                continue;
            }
            
            newLocationsQueue.Enqueue(location); // Not created yet location
        }
        
        _locationsQueue.Clear();
        
        foreach (var location in newLocationsQueue)
        {
            _locationsQueue.Enqueue(location);
        }
        
        _lastSendTime = DateTime.UtcNow;
        _positionsSent += createdLocationsIds.Count;
    }
    
    private void OnNotificationUpdateTimer(object sender, ElapsedEventArgs e)
    {
        SendStatisticsNotification();
    }
    
    private void SendStatisticsNotification()
    {
        var currentTime = DateTime.UtcNow;
        
        var lastFixTimeString = _lastFixTime.HasValue
            ? $"{(currentTime - _lastFixTime.Value):hh\\:mm\\:ss} ago"
            : "N/A"; // TODO: Localize me
        
        var lastSendTimeString = _lastSendTime.HasValue
            ? $"{(currentTime - _lastSendTime.Value):hh\\:mm\\:ss} ago"
            : "N/A"; // TODO: Localize me

        var remainingQueueSize = _locationsQueue.Count;
        
        var message = @$"Last GPS fix { lastFixTimeString }
Last data submission { lastSendTimeString }
Positions sent: { _positionsSent }
Positions to send: { remainingQueueSize }";
        
        _notificationsService.ShowNotification(GlobalConstants.TrackingIsOnNotificationTitle, message, 0, true);
    }
}