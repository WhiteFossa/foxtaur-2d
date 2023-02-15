namespace LibGeo.Models;

/// <summary>
/// Point with geographic coordinates
/// </summary>
public class GeoPoint
{
    /// <summary>
    /// Latitude (radians)
    /// </summary>
    public double Lat { get; private set; }

    /// <summary>
    /// Longitude (radians)
    /// </summary>
    public double Lon { get; private set; }

    public GeoPoint(double lat, double lon)
    {
        if (lat > Math.PI / 2.0 || lat < -1.0 * Math.PI / 2.0)
        {
            throw new ArgumentOutOfRangeException(nameof(lat));
        }

        if (lon > Math.PI || lon < -1.0 * Math.PI)
        {
            throw new ArgumentOutOfRangeException(nameof(lon));
        }

        Lat = lat;
        Lon = lon;
    }
}