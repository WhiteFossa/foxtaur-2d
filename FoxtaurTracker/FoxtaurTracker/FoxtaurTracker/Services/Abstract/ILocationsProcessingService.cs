using FoxtaurTracker.Services.Abstract.Models;

namespace FoxtaurTracker.Services.Abstract;

/// <summary>
/// Delegate for statistics update event
/// </summary>
public delegate void OnStatisticsUpdate(LocationsServiceStatistics statistics);

/// <summary>
/// Service for processing and sending geocoordinates
/// </summary>
public interface ILocationsProcessingService
{
    /// <summary>
    /// Start tracking
    /// </summary>
    Task StartTrackingAsync(OnStatisticsUpdate statisticsCallback);

    /// <summary>
    /// Stop tracking
    /// </summary>
    Task StopTrackingAsync();

    /// <summary>
    /// Return true if "access location always" permission is granted
    /// </summary>
    Task<bool> CheckForLocationAlwaysPermissionAsync();

    /// <summary>
    /// Return true if tracking active
    /// </summary>
    bool IsTrackingOn();
}