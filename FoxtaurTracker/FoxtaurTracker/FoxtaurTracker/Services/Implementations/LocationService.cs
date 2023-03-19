using FoxtaurTracker.Constants;
using FoxtaurTracker.Services.Abstract;
using FoxtaurTracker.Services.Abstract.Enums;
using FoxtaurTracker.Services.Abstract.Models;
using LibAuxiliary.Helpers;

namespace FoxtaurTracker.Services.Implementations;

public class LocationService : ILocationService
{
    private CancellationTokenSource _cancelTokenSource;

    public async Task GetCurrentLocationAsync(OnLocationObtainedAsync callback)
    {
        Location location = null;
        try
        {
            var locationReqeust = new GeolocationRequest(GeolocationAccuracy.Best, TimeSpan.FromSeconds(GlobalConstants.GpsTimeout));

            _cancelTokenSource = new CancellationTokenSource();

            location = await Geolocation.Default.GetLocationAsync(locationReqeust, _cancelTokenSource.Token);

            if (location == null)
            {
                await callback(new GetLocationArgs(GetLocationStatus.TimedOut, DateTime.UtcNow, 0.0, 0.0, 0.0));
                return;
            }
            
        }
        catch (FeatureNotSupportedException)
        {
            // Device have no GPS
            await callback(new GetLocationArgs(GetLocationStatus.NotSupported, DateTime.UtcNow, 0.0, 0.0, 0.0));
            return;
        }
        catch (FeatureNotEnabledException)
        {
            // GPS not enabled
            await callback(new GetLocationArgs(GetLocationStatus.NotEnabled, DateTime.UtcNow, 0.0, 0.0, 0.0));
            return;
        }
        catch (PermissionException)
        {
            // User didn't give the permission
            await callback(new GetLocationArgs(GetLocationStatus.NotPermitted, DateTime.UtcNow, 0.0, 0.0, 0.0));
            return;
        }
        
        // Success
        await callback(new GetLocationArgs
        (
            GetLocationStatus.Success,
            location.Timestamp.UtcDateTime,
            location.Latitude.ToRadians(),
            location.Longitude.ToRadians(),
            location.Altitude.HasValue ? location.Altitude.Value : 0.0));
    }
}