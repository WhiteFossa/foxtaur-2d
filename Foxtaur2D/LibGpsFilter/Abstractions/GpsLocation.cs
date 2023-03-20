namespace LibGpsFilter.Abstractions;

/// <summary>
/// GPS location for filter
/// </summary>
public class GpsLocation
{
    /// <summary>
    /// Timestamp
    /// </summary>
    public DateTimeOffset Timestamp { get; }

    /// <summary>
    /// Latitude
    /// </summary>
    public double Lat { get; }

    /// <summary>
    /// Longitude
    /// </summary>
    public double Lon { get; }

    public GpsLocation(DateTimeOffset timestamp, double lat, double lon)
    {
        Timestamp = timestamp;
        Lat = lat;
        Lon = lon;
    }
}