using System.Collections.Concurrent;
using System.Timers;
using FoxtaurTracker.Constants;
using FoxtaurTracker.Services.Abstract;
using FoxtaurTracker.Services.Abstract.Enums;
using FoxtaurTracker.Services.Abstract.Models;
using LibWebClient.Models;
using LibWebClient.Models.DTOs;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;
using Timer = System.Timers.Timer;

namespace FoxtaurTracker.Services.Implementations;

public class LocationsProcessingService : ILocationsProcessingService
{
    private ILocationService _locationService;
    private readonly IWebClient _webClient;
    
    /// <summary>
    /// New coordinates are requested by this timer
    /// </summary>
    private Timer _locationFetchTimer;
    
    /// <summary>
    /// Coordinates queue is being checked by this timer
    /// </summary>
    private Timer _locationsSendTimer;

    private ConcurrentQueue<HunterLocation> _locationsQueue = new ConcurrentQueue<HunterLocation>();

    public LocationsProcessingService(ILocationService locationService,
        IWebClient webClient)
    {
        _locationService = locationService;
        _webClient = webClient;
        
        // Setting up location fetch timer
        _locationFetchTimer = new Timer(TimeSpan.FromSeconds(GlobalConstants.LocationFetchInterval));
        _locationFetchTimer.Elapsed += OnLocationFetchTimer;
        _locationFetchTimer.AutoReset = true;
        
        // Setting up locations send timer
        _locationsSendTimer = new Timer(TimeSpan.FromSeconds(GlobalConstants.LocationsSendInterval));
        _locationsSendTimer.Elapsed += OnLocationSendTimer;
        _locationsSendTimer.AutoReset = true;
        _locationsSendTimer.Enabled = true;
    }
    
    public async Task StartTrackingAsync()
    {
        _locationFetchTimer.Start();
        _locationsQueue.Clear();
    }

    public async Task StopTrackingAsync()
    {
        _locationFetchTimer.Stop();
    }

    private void OnLocationFetchTimer(object sender, ElapsedEventArgs e)
    {
        _locationService.GetCurrentLocationAsync(OnLocationObtainedAsync); // INTENTIONALLY run without async
    }
    
    private async Task OnLocationObtainedAsync(GetLocationArgs args)
    {
        if (args.Status == GetLocationStatus.NotSupported)
        {
            await StopTrackingAsync();
            await MainThread.InvokeOnMainThreadAsync(async () => await App.PopupsService.ShowAlertAsync("Error", "This device have no GPS receiver."));
            return;
        }
        else if (args.Status == GetLocationStatus.NotEnabled)
        {
            await StopTrackingAsync();
            await MainThread.InvokeOnMainThreadAsync(async () => await App.PopupsService.ShowAlertAsync("Error", "Please enable location."));
            return;
        }
        else if (args.Status == GetLocationStatus.NotPermitted)
        {
            await StopTrackingAsync();
            await MainThread.InvokeOnMainThreadAsync(async () => await App.PopupsService.ShowAlertAsync("Error", "Please give this application location permission."));
            return;
        }
        else if (args.Status == GetLocationStatus.TimedOut)
        {
            return; // Giving another attempt
        }

        var hunterLocation = new HunterLocation(Guid.NewGuid(), args.Timestamp, args.Lat, args.Lon, args.Alt);
        _locationsQueue.Enqueue(hunterLocation);
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

        IReadOnlyCollection<HunterLocation> createdLocations = null;
        try
        {
            createdLocations = _webClient.CreateHunterLocationsAsync(request).Result;
        }
        catch (Exception)
        {
            // Swallowing excetion
            // TODO: Add logging
            return;
        }

        // Regenerating queue
        var newLocationsQueue = new ConcurrentQueue<HunterLocation>();

        while (_locationsQueue.Any())
        {
            _locationsQueue.TryDequeue(out var location);
            
            if (createdLocations.Any(cl => cl.Id == location.Id))
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
    }
}