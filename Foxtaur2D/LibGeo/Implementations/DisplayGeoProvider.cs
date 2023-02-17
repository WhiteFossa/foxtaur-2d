using LibGeo.Abstractions;
using LibGeo.Models;

namespace LibGeo.Implementations;

/// <summary>
/// Geo provider to convert display coordinates
/// </summary>
public class DisplayGeoProvider : IGeoProvider
{
    private double _baseLat;
    private double _baseLon;
    private double _resolution;
    
    /// <summary>
    /// Latitude for Y = 0
    /// </summary>
    public double BaseLat
    {
        get
        {
            return _baseLat;
        }
        set
        {
            _baseLat = value;
        }
    }

    /// <summary>
    /// Longitude for X = 0
    /// </summary>
    public double BaseLon
    {
        get
        {
            return _baseLon;
        }
        set
        {
            _baseLon = value;
        }
    }

    /// <summary>
    /// Radians per pixel
    /// </summary>
    public double Resolution
    {
        get
        {
            return _resolution;
        }
        set
        {
            _resolution = value;
        }
    }

    public double LonToX(double lon)
    {
        return (lon - _baseLon) / _resolution;
    }

    public double LatToY(double lat)
    {
        return (_baseLat - lat) / _resolution;
    }

    public PlanarPoint GeoToPlanar(GeoPoint geo)
    {
        return new PlanarPoint(LonToX(geo.Lon), LatToY(geo.Lat));
    }

    public double YToLat(double y)
    {
        return _baseLat - _resolution * y;
    }

    public double XToLon(double x)
    {
        return _baseLon + _resolution * x;
    }

    public GeoPoint PlanarToGeo(PlanarPoint planar)
    {
        return new GeoPoint(YToLat(planar.Y), XToLon(planar.X));
    }
}