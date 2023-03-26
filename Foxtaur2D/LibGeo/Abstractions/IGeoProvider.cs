using LibGeo.Models;

namespace LibGeo.Abstractions;

/// <summary>
/// Geography conversion provider
/// </summary>
public interface IGeoProvider
{
    /// <summary>
    /// Longitude to X
    /// </summary>
    double LonToX(double lon);
    
    /// <summary>
    /// Latitude to Y
    /// </summary>
    double LatToY(double lat);
    
    /// <summary>
    /// Geographic coordinates to planar coordinates
    /// </summary>
    PlanarPoint GeoToPlanar(GeoPoint geo);

    /// <summary>
    /// Y coordinate to latitude
    /// </summary>
    double YToLat(double y);

    /// <summary>
    /// X coordinate to longitude
    /// </summary>
    double XToLon(double x);
    
    /// <summary>
    /// Planar coordinates to geo coordinates
    /// </summary>
    GeoPoint PlanarToGeo(PlanarPoint planar);

    /// <summary>
    /// Get distance by pixels count
    /// </summary>
    double GetDistanceByPixelsCount(double pixelsCount);

    /// <summary>
    /// Get pixels count by distance
    /// </summary>
    double GetPixelsCountByDistance(double distance);
}