using LibGeo.Abstractions;
using LibGeo.Models;

namespace LibGeo.Implementations;

/// <summary>
/// Geography provider for flat surface (equidistant)
/// </summary>
public class FlatGeoProvider : IGeoProvider
{
    private readonly double _dx;
    private readonly double _dy;

    public FlatGeoProvider(double width, double height)
    {
        if (width <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(width));
        }

        if (height <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(height));
        }

        _dx = width / (2.0 * Math.PI);
        _dy = height / Math.PI;
    }

    public double LonToX(double lon)
    {
        return (Math.PI + lon) * _dx;
    }

    public double LatToY(double lat)
    {
        return (Math.PI / 2.0 - lat) * _dy;
    }

    public PlanarPoint GeoToPlanar(GeoPoint geo)
    {
        return new PlanarPoint(LonToX(geo.Lon), LatToY(geo.Lat));
    }

    public double YToLat(double y)
    {
        return Math.PI / 2.0 - y / _dy;
    }

    public double XToLon(double x)
    {
        return x / _dx - Math.PI;
    }

    public GeoPoint PlanarToGeo(PlanarPoint planar)
    {
        return new GeoPoint(YToLat(planar.Y), XToLon(planar.X));
    }
}