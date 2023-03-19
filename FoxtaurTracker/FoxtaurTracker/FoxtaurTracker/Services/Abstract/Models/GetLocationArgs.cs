using FoxtaurTracker.Services.Abstract.Enums;

namespace FoxtaurTracker.Services.Abstract.Models;

/// <summary>
/// Arguments for get location callback
/// </summary>
public class GetLocationArgs
{
    /// <summary>
    /// Location status
    /// </summary>
    public GetLocationStatus Status { get; }

    /// <summary>
    /// When location was obtained
    /// </summary>
    public DateTime Timestamp { get; }

    /// <summary>
    /// Latitude in radians
    /// </summary>
    public double Lat { get; }

    /// <summary>
    /// Longitude in radians
    /// </summary>
    public double Lon { get; }

    /// <summary>
    /// Altitude in meters
    /// </summary>
    public double Alt { get; }

    public GetLocationArgs
    (
        GetLocationStatus status,
        DateTime timestamp,
        double lat,
        double lon,
        double alt
    )
    {
        Status = status;
        Timestamp = timestamp;
        Lat = lat;
        Lon = lon;
        Alt = alt;
    }
}