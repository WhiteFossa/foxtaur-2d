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
}