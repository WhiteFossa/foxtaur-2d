using LibGeo.Abstractions;
using LibGeo.Constants;
using LibGeo.Models;

namespace LibGeo.Implementations;

/// <summary>
/// Geo provider to convert display coordinates
/// </summary>
public class DisplayGeoProvider : IGeoProvider
{
    private double _baseLat;
    private double _baseLon;

    private double _screenWidth;
    private double _screenHeight;

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

            if (_baseLat - Resolution * _screenHeight < -1.0 * Math.PI / 2.0)
            {
                _baseLat = -1.0 * Math.PI / 2.0 + Resolution * _screenHeight;
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

            if (_baseLon + Resolution * _screenWidth > Math.PI)
            {
                _baseLon = Math.PI - Resolution * _screenWidth;
            }
        }
    }

    /// <summary>
    /// Radians per pixel
    /// </summary>
    public double Resolution { get; private set; } = 0.0005;

    public DisplayGeoProvider(double screenWidth, double screenHeight)
    {
        BaseLat = Math.PI / 2.0;
        BaseLon = -1 * Math.PI;
        _screenWidth = screenWidth;
        _screenHeight = screenHeight;

        Resolution = CalculateMaxResolution();
    }
    
    public double LonToX(double lon)
    {
        return (lon - _baseLon) / Resolution;
    }

    public double LatToY(double lat)
    {
        return (_baseLat - lat) / Resolution;
    }

    public PlanarPoint GeoToPlanar(GeoPoint geo)
    {
        return new PlanarPoint(LonToX(geo.Lon), LatToY(geo.Lat));
    }

    public double YToLat(double y)
    {
        return _baseLat - Resolution * y;
    }

    public double XToLon(double x)
    {
        return _baseLon + Resolution * x;
    }

    public GeoPoint PlanarToGeo(PlanarPoint planar)
    {
        return new GeoPoint(YToLat(planar.Y), XToLon(planar.X));
    }

    public double GetDistanceByPixelsCount(double pixelsCount)
    {
        return pixelsCount * Resolution * GeoConstants.MetersPerRadian;
    }

    public double GetPixelsCountByDistance(double distance)
    {
        return distance / (Resolution * GeoConstants.MetersPerRadian);
    }

    /// <summary>
    /// Move display on mouse move. Old coordinates are where mouse was pressed, new coordinates are current mouse coordinates
    /// </summary>
    public void MoveDisplay(double oldX, double oldY, double newX, double newY)
    {
        BaseLat -= Resolution * (oldY - newY);
        BaseLon += Resolution * (oldX - newX);
    }

    /// <summary>
    /// Zoom display to a new resolution (mouse is in x, y position)
    /// </summary>
    public void Zoom(double newResolution, double x, double y)
    {
        var oldResolution = Resolution;

        Resolution = newResolution;
        
        // Limiting resolution
        var maxResolution = CalculateMaxResolution();
        
        if (Resolution > maxResolution)
        {
            Resolution = maxResolution;
        }
        
        // Correcting base coordinates
        BaseLat -= y * (oldResolution - Resolution);
        BaseLon += x * (oldResolution - Resolution);
    }

    /// <summary>
    /// Center display at given geocoordinates
    /// </summary>
    public void CenterDisplay(double centerLat, double centerLon)
    {
        BaseLat = centerLat + Resolution * _screenHeight / 2.0;
        BaseLon = centerLon - Resolution * _screenWidth / 2.0;
    }
    
    private double CalculateMaxResolution()
    {
        var maxResolutionLat = (BaseLat + Math.PI / 2.0) / _screenHeight;
        var maxResolutionLon = (Math.PI - BaseLon) / _screenWidth;
        return Math.Min(maxResolutionLat, maxResolutionLon);
    }
}