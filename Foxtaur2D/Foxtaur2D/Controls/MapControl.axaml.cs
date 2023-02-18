using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using DynamicData.Kernel;
using ImageMagick;
using LibGeo.Abstractions;
using LibGeo.Implementations;
using LibGeo.Models;
using LibRenderer.Abstractions;
using LibRenderer.Constants;
using LibRenderer.Implementations;
using NLog;

namespace Foxtaur2D.Controls;

public partial class MapControl : UserControl
{
    #region Control sizes
    
    /// <summary>
    /// Screen scaling
    /// </summary>
    private double _scaling;

    /// <summary>
    /// Viewport width
    /// </summary>
    private int _viewportWidth;

    /// <summary>
    /// Viewport height
    /// </summary>
    private int _viewportHeight;
    
    #endregion
    
    #region Drawing

    /// <summary>
    /// Backing image array
    /// </summary>
    private Byte[] _backingArray;

    /// <summary>
    /// Layers to display
    /// </summary>
    private List<ILayer> _layers = new List<ILayer>();
    
    /// <summary>
    /// Backing image geoprovider
    /// </summary>
    private IGeoProvider _backingImageGeoProvider;
    
    #endregion
    
    /// <summary>
    /// Logger
    /// </summary>
    private Logger _logger = LogManager.GetCurrentClassLogger();
    
    #region Debug

    #endregion
    
    public MapControl()
    {
        InitializeComponent();

        _backingArray = null; // It will remain null till the first resize

        _layers.Add(new FlatImageLayer("Resources/HYP_50M_SR_W.tif"));
        //  _layers.Add(new ImageLayer("Resources/Gorica.tif"));
        
        // Listening for properties changes to process resize
        PropertyChanged += OnPropertyChangedListener;
        
        // Setting-up input events
        PointerMoved += OnMouseMoved;
    }

    private void OnMouseMoved(object? sender, PointerEventArgs e)
    {
        var x = e.GetCurrentPoint(this).Position.X * _scaling;
        var y = e.GetCurrentPoint(this).Position.Y * _scaling;

        (_backingImageGeoProvider as DisplayGeoProvider).BaseLat = Math.PI / 2.0 - (_viewportHeight - y) / 1000.0;
        (_backingImageGeoProvider as DisplayGeoProvider).BaseLon = -1 * Math.PI + (_viewportWidth - x) / 1000.0;
        
        InvalidateVisual();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    /// <summary>
    /// Properties change listener
    /// </summary>
    private void OnPropertyChangedListener(object sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property.Name.Equals("Bounds"))
        {
            // Resize event
            OnResize((Rect)e.NewValue);
        }
    }
    
    /// <summary>
    /// Called when control resized
    /// </summary>
    private void OnResize(Rect bounds)
    {
        _scaling = VisualRoot.RenderScaling;

        _viewportWidth = (int)(bounds.Width * _scaling);
        _viewportHeight = (int)(bounds.Height * _scaling);
        
        // Recreating backing array
        _backingArray = new byte[_viewportWidth * _viewportHeight * 4];
        
        // Re-setup backing image geoprovider
        _backingImageGeoProvider = new DisplayGeoProvider();
        (_backingImageGeoProvider as DisplayGeoProvider).BaseLat = Math.PI / 2.0;
        (_backingImageGeoProvider as DisplayGeoProvider).BaseLon = -1 * Math.PI;
        (_backingImageGeoProvider as DisplayGeoProvider).Resolution = 0.0005;
    }
    
    /// <summary>
    /// Render the control
    /// </summary>
    public override unsafe void Render(DrawingContext context)
    {
        foreach (var layer in _layers)
        {
            var layerPixels = layer.GetPixelsArray();
            for (var y = 0; y < _viewportHeight; y++)
            {
                Parallel.For(0, _viewportWidth,
                x =>
                {
                    var backingLat = _backingImageGeoProvider.YToLat(y);
                    var backingLon = _backingImageGeoProvider.XToLon(x);
                    var backingIndex = (y * _viewportWidth + x) * 4;

                    var layerX = layer.GeoProvider.LonToX(backingLon);
                    var layerY = layer.GeoProvider.LatToY(backingLat);
                    var layerIndex = ((int)layerY * layer.Width + (int)layerX) * 4;

                    var opacity = layerPixels[layerIndex + 3] / (double)0xFF;

                    _backingArray[backingIndex] = MixBrightness(layerPixels[layerIndex], _backingArray[backingIndex], opacity);
                    _backingArray[backingIndex + 1] = MixBrightness(layerPixels[layerIndex + 1], _backingArray[backingIndex + 1], opacity);
                    _backingArray[backingIndex + 2] = MixBrightness(layerPixels[layerIndex + 2], _backingArray[backingIndex + 2], opacity);
                    _backingArray[backingIndex + 3] = 0xFF;
                });
            }
        }
        
        // Rendering backing image
        fixed (byte* pixels = _backingArray)
        {
            var bitmapToDraw = new Bitmap(PixelFormat.Rgba8888, (nint)pixels, new PixelSize(_viewportWidth, _viewportHeight), new Vector(RendererConstants.DefaultDPI / _scaling, RendererConstants.DefaultDPI / _scaling), _viewportWidth * 4);
            context.DrawImage(bitmapToDraw, new Rect(0, 0, _viewportWidth, _viewportHeight));
        }

        base.Render(context);
    }

    private byte MixBrightness(byte topBrightness, byte bottomBrightness, double multiplier)
    {
        int newBrightness = (int)Math.Floor(topBrightness * multiplier + bottomBrightness * (1 - multiplier) + 0.5);
        if (newBrightness > 0xFF)
        {
            newBrightness = 0xFF;
        }
        else if (newBrightness < 0)
        {
            newBrightness = 0;
        }

        return (byte)newBrightness;
    }
}
