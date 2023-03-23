namespace FoxtaurTracker.Services.Abstract.Models;

/// <summary>
/// Statistics about locations
/// </summary>
public class LocationsServiceStatistics
{
    /// <summary>
    /// Last GPS fix was this time ago. Null if time unknown
    /// </summary>
    public TimeSpan? LastGpsFix { get; private set; }

    /// <summary>
    /// Last data was sent to server this time ago. Null if time unknown
    /// </summary>
    public TimeSpan? LastDataSendTime { get; private set; }

    /// <summary>
    /// This amounf of locations was sent to server during tracking session
    /// </summary>
    public int LocationsSent { get; private set; }

    /// <summary>
    /// Locations-to-send queue size
    /// </summary>
    public int LocationsToSend { get; private set; }

    public LocationsServiceStatistics
    (
        TimeSpan? lastGpsFix,
        TimeSpan? lastDataSendTime,
        int locationsSent,
        int locationsToSend
    )
    {
        LastGpsFix = lastGpsFix;
        LastDataSendTime = lastDataSendTime;
        LocationsSent = locationsSent;
        LocationsToSend = locationsToSend;
    }
}