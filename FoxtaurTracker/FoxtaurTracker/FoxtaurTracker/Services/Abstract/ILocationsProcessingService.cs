namespace FoxtaurTracker.Services.Abstract;

/// <summary>
/// Service for processing and sending geocoordinates
/// </summary>
public interface ILocationsProcessingService
{
    /// <summary>
    /// Start tracking
    /// </summary>
    Task StartTrackingAsync();

    /// <summary>
    /// Stop tracking
    /// </summary>
    Task StopTrackingAsync();

    /// <summary>
    /// Return true if "access location always" permission is granted
    /// </summary>
    Task<bool> CheckForLocationAlwaysPermissionAsync();
}