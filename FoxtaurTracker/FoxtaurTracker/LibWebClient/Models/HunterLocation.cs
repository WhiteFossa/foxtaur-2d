namespace LibWebClient.Models;

/// <summary>
/// Hunter location
/// </summary>
public class HunterLocation
{
    /// <summary>
    /// Location ID
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Timestamp
    /// </summary>
    public DateTime Timestamp { get; }

    /// <summary>
    /// Latitude
    /// </summary>
    public double Lat { get; }

    /// <summary>
    /// Longitude
    /// </summary>
    public double Lon { get; }

    /// <summary>
    /// Altitude
    /// </summary>
    public double Alt { get; }

    public HunterLocation(Guid id, DateTime timestamp, double lat, double lon, double alt)
    {
        Id = id;
        Timestamp = timestamp;
        Lat = lat;
        Lon = lon;
        Alt = alt;
    }
}