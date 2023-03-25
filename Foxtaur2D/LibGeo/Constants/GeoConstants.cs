namespace LibGeo.Constants;

/// <summary>
/// Constants for geoproviders etc
/// </summary>
public static class GeoConstants
{
    /// <summary>
    /// Equatorial big circle length in meters
    /// </summary>
    public const double BigCircleLength = 40075017;

    /// <summary>
    /// Meters per radian (longitudinal direction, at equator)
    /// </summary>
    public const double MetersPerRadian = BigCircleLength / (2.0 * Math.PI);

    /// <summary>
    /// Minimal pixel size (defines max zoom)
    /// </summary>
    public const double MinPixelSize = (0.1 / BigCircleLength) * 2.0 * Math.PI; // 10cm per pixel
}