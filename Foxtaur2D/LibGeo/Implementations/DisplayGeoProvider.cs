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

            if (_baseLat - PixelSize * _screenHeight < -1.0 * Math.PI / 2.0)
            {
                _baseLat = -1.0 * Math.PI / 2.0 + PixelSize * _screenHeight;
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

            if (_baseLon + PixelSize * _screenWidth > Math.PI)
            {
                _baseLon = Math.PI - PixelSize * _screenWidth;
            }
        }
    }

    /// <summary>
    /// Radians per pixel
    /// </summary>
    public double PixelSize { get; private set; }

    public DisplayGeoProvider(double screenWidth, double screenHeight)
    {
        BaseLat = Math.PI / 2.0;
        BaseLon = -1 * Math.PI;
        _screenWidth = screenWidth;
        _screenHeight = screenHeight;

        PixelSize = CalculateMinPixelSize();
    }
    
    public double LonToX(double lon)
    {
        return (lon - _baseLon) / PixelSize;
    }

    public double LatToY(double lat)
    {
        return (_baseLat - lat) / PixelSize;
    }

    public PlanarPoint GeoToPlanar(GeoPoint geo)
    {
        return new PlanarPoint(LonToX(geo.Lon), LatToY(geo.Lat));
    }

    public double YToLat(double y)
    {
        return _baseLat - PixelSize * y;
    }

    public double XToLon(double x)
    {
        return _baseLon + PixelSize * x;
    }

    public GeoPoint PlanarToGeo(PlanarPoint planar)
    {
        return new GeoPoint(YToLat(planar.Y), XToLon(planar.X));
    }

    public double GetDistanceByPixelsCount(double pixelsCount)
    {
        return pixelsCount * PixelSize * GeoConstants.MetersPerRadian;
    }

    public double GetPixelsCountByDistance(double distance)
    {
        return distance / (PixelSize * GeoConstants.MetersPerRadian);
    }

    /// <summary>
    /// Move display on mouse move. Old coordinates are where mouse was pressed, new coordinates are current mouse coordinates
    /// </summary>
    public void MoveDisplay(double oldX, double oldY, double newX, double newY)
    {
        BaseLat -= PixelSize * (oldY - newY);
        BaseLon += PixelSize * (oldX - newX);
    }

    /// <summary>
    /// Call this if control size changed
    /// </summary>
    public void OnResize(double newScreenWidth, double newScreenHeight)
    {
        var centerLat = YToLat(_screenHeight / 2.0);
        var centerLon = XToLon(_screenWidth / 2.0);
        
        _screenWidth = newScreenWidth;
        _screenHeight = newScreenHeight;

        // Limiting resolution
        var maxResolution = CalculateMinPixelSize();
        
        if (PixelSize > maxResolution)
        {
            PixelSize = maxResolution;
        }

        if (PixelSize < GeoConstants.MinPixelSize)
        {
            PixelSize = GeoConstants.MinPixelSize;
        }
        
        CenterDisplay(centerLat, centerLon);
    }
    
    /// <summary>
    /// Zoom display to a new resolution (mouse is in x, y position)
    /// </summary>
    public void Zoom(double newPixelSize, double x, double y)
    {
        var oldPixelSize = PixelSize;

        PixelSize = newPixelSize;
        
        // Limiting resolution
        var maxResolution = CalculateMinPixelSize();
        
        if (PixelSize > maxResolution)
        {
            PixelSize = maxResolution;
        }

        if (PixelSize < GeoConstants.MinPixelSize)
        {
            PixelSize = GeoConstants.MinPixelSize;
        }
        
        // Correcting base coordinates
        BaseLat -= y * (oldPixelSize - PixelSize);
        BaseLon += x * (oldPixelSize - PixelSize);
    }

    /// <summary>
    /// Zoom provider such way, that rectangle area, limited by given points, will fill the whole screen
    /// </summary>
    public void ZoomTo(GeoPoint northWest, GeoPoint southEast)
    {
        var deltaLat = northWest.Lat - southEast.Lat;
        var deltaLon = southEast.Lon - northWest.Lon;

        var pixelSizeX = deltaLon / _screenWidth;
        var pixelSizeY = deltaLat / _screenHeight;
        Zoom(Math.Max(pixelSizeX, pixelSizeY), 0, 0);
    }

    /// <summary>
    /// Center display at given geocoordinates
    /// </summary>
    public void CenterDisplay(double centerLat, double centerLon)
    {
        BaseLat = centerLat + PixelSize * _screenHeight / 2.0;
        BaseLon = centerLon - PixelSize * _screenWidth / 2.0;
    }
    
    private double CalculateMinPixelSize()
    {
        var minPixelSizeLat = (BaseLat + Math.PI / 2.0) / _screenHeight;
        var minPixelSizeLon = (Math.PI - BaseLon) / _screenWidth;
        return Math.Min(minPixelSizeLat, minPixelSizeLon);
    }
}