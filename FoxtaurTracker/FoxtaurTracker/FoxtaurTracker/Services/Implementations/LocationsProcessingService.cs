using System.Timers;
using FoxtaurTracker.Constants;
using FoxtaurTracker.Services.Abstract;
using FoxtaurTracker.Services.Abstract.Models;
using Timer = System.Timers.Timer;

namespace FoxtaurTracker.Services.Implementations;

public class LocationsProcessingService : ILocationsProcessingService
{
    private ILocationService _locationService;
    
    /// <summary>
    /// New coordinates are requested by this timer
    /// </summary>
    private Timer _locationFetchTimer;

    public LocationsProcessingService(ILocationService locationService)
    {
        _locationService = locationService;
        
        // Setting up location fetch timer
        _locationFetchTimer = new Timer(GlobalConstants.LocationFetchInterval);
        _locationFetchTimer.Elapsed += OnLocationFetchTimer;
        _locationFetchTimer.AutoReset = true;
    }
    
    public async Task StartTrackingAsync()
    {
        _locationFetchTimer.Start();
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
        int a = 10;
    }
}