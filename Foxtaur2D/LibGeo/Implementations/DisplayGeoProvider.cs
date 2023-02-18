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

    public double ScreenWidth { get; set; }

    public double ScreenHeight { get; set; }

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

            if (_baseLat > Math.PI / 2.0)
            {
                _baseLat = Math.PI / 2.0;
            }

            if (_baseLat - Resolution * ScreenHeight < -1.0 * Math.PI / 2.0)
            {
                _baseLat = -1.0 * Math.PI / 2.0 + Resolution * ScreenHeight;
            }
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

            if (_baseLon < -1 * Math.PI)
            {
                _baseLon = -1 * Math.PI;
            }

            if (_baseLon + Resolution * ScreenWidth > Math.PI)
            {
                _baseLon = Math.PI - Resolution * ScreenWidth;
            }
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

    /// <summary>
    /// Move display on mouse move. Old coordinates are where mouse was pressed, new coordinates are current mouse coordinates
    /// </summary>
    public void MoveDisplay(double oldX, double oldY, double newX, double newY)
    {
        BaseLat -= Resolution * (oldY - newY);
        BaseLon += Resolution * (oldX - newX);
    }
}