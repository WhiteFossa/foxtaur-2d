using Avalonia;
using Avalonia.Media;
using LibGeo.Abstractions;
using LibRenderer.Abstractions.Drawers;
using LibRenderer.Abstractions.Layers;
using LibRenderer.Constants;
using LibResources.Implementations.Resources;
using LibWebClient.Models;
using Microsoft.Extensions.DependencyInjection;

namespace LibRenderer.Implementations.Layers;

/// <summary>
/// Delegate for distance loaded event
/// </summary>
public delegate void OnDistanceLoaded();

public class DistanceLayer : IVectorLayer, IRasterLayer
{
    private readonly ITextDrawer _textDrawer;
    
    private readonly Distance _distance;

    private bool _isMapLoaded;
    
    private CompressedStreamResource _mapImage;
    private GeoTiffLayer _mapLayer;

    private Mutex _isReadyLock = new Mutex();

    private OnDistanceLoaded _onDistanceLoadedEvent;

    /// <summary>
    /// OnDistanceLoadedEvent is called on separate thread!
    /// </summary>
    public DistanceLayer(Distance distanceModel, OnDistanceLoaded onDistanceLoadedEvent, ITextDrawer textDrawer)
    {
        _textDrawer = textDrawer ?? throw new ArgumentNullException(nameof(textDrawer));
        _distance = distanceModel ?? throw new ArgumentNullException(nameof(distanceModel));
        _onDistanceLoadedEvent = onDistanceLoadedEvent ?? throw new ArgumentNullException(nameof(onDistanceLoadedEvent));

        // Starting to download a map
        _isMapLoaded = false;
        _mapImage = new CompressedStreamResource(_distance.Map.Url, false);
        
        var downloadThread = new Thread(() => _mapImage.Download(OnMapImageLoaded));
        downloadThread.Start();
    }
    
    public void Draw(DrawingContext context, int width, int height, double scalingFactor, IGeoProvider displayGeoProvider)
    {
        // Distance bounds
        var topY = displayGeoProvider.LatToY(_distance.Map.NorthLat) / scalingFactor;
        var bottomY = displayGeoProvider.LatToY(_distance.Map.SouthLat) / scalingFactor;
        var leftX = displayGeoProvider.LonToX(_distance.Map.WestLon) / scalingFactor;
        var rightX = displayGeoProvider.LonToX(_distance.Map.EastLon) / scalingFactor;
        
        context.DrawRectangle(new SolidColorBrush(Colors.Transparent), // Always transparent, no need for a constant
            new Pen(new SolidColorBrush(RendererConstants.DistanceBorderColor), RendererConstants.DistanceBorderThickness),
            new Rect(new Point(leftX, topY), new Point(rightX, bottomY)));
        
        // Distance name
        var formattedDistanceName = new FormattedText(_distance.Name,
            Typeface.Default,
            RendererConstants.DistanceNameFontSize,
            TextAlignment.Left,
            TextWrapping.NoWrap,
            new Size(double.MaxValue, double.MaxValue));
        
        context.DrawText(new SolidColorBrush(RendererConstants.DistanceNameColor),
            new Point(leftX, topY - formattedDistanceName.Bounds.Height),
            formattedDistanceName);
        
        // Finish
        DrawFinish(_distance.FinishLocation, context, width, height, scalingFactor, displayGeoProvider);
    }

    /// <summary>
    /// Draw finish location
    /// </summary>
    private void DrawFinish(Location finishLocation, DrawingContext context, int width, int height, double scalingFactor, IGeoProvider displayGeoProvider)
    {
        var finishX = displayGeoProvider.LonToX(finishLocation.Lon) / scalingFactor;
        var finishY = displayGeoProvider.LatToY(finishLocation.Lat) / scalingFactor;
        
        // Outer circle
        context.DrawEllipse(new SolidColorBrush(Colors.Transparent),
            new Pen(new SolidColorBrush(RendererConstants.FinishColor), RendererConstants.FinishPenThickness),
            new Point(finishX, finishY),
            RendererConstants.FinishOuterRadius,
            RendererConstants.FinishOuterRadius);
        
        // Inner circle
        context.DrawEllipse(new SolidColorBrush(Colors.Transparent),
            new Pen(new SolidColorBrush(RendererConstants.FinishColor), RendererConstants.FinishPenThickness),
            new Point(finishX, finishY),
            RendererConstants.FinishInnerRadius,
            RendererConstants.FinishInnerRadius);
        
        // Name
        var formattedName = new FormattedText(finishLocation.Name,
            Typeface.Default,
            RendererConstants.FinishNameFontSize,
            TextAlignment.Left,
            TextWrapping.NoWrap,
            new Size(double.MaxValue, double.MaxValue));
        
        context.DrawText(new SolidColorBrush(RendererConstants.FinishColor),
            new Point(finishX - formattedName.Bounds.Width / 2.0, finishY - RendererConstants.FinishOuterRadius - formattedName.Bounds.Height),
            formattedName);
    }
    
    private void OnMapImageLoaded(DownloadableResourceBase resource)
    {
        var imageResource = resource as CompressedStreamResource;
        _mapLayer = new GeoTiffLayer(imageResource.DecompressedStream, _textDrawer);
        _mapLayer.Load();
        
        // Map is ready
        _isReadyLock.WaitOne();
        
        try
        {
            _isMapLoaded = true;
        }
        finally
        {
            _isReadyLock.ReleaseMutex();
        }

        _onDistanceLoadedEvent();
    }

    public int Width => _mapLayer.Width;

    public int Height => _mapLayer.Height;

    public void RegeneratePixelsArray()
    {
        _mapLayer.RegeneratePixelsArray();
    }

    public byte[] GetPixelsArray()
    {
        return _mapLayer.GetPixelsArray();
    }

    public bool GetPixelCoordinates(double lat, double lon, out double x, out double y)
    {
        return _mapLayer.GetPixelCoordinates(lat, lon, out x, out y);
    }

    public bool IsReady()
    {
        _isReadyLock.WaitOne();

        try
        {
            return _isMapLoaded;
        }
        finally
        {
            _isReadyLock.ReleaseMutex();
        }
        
    }
}