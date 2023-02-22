using System;
using System.Collections.Generic;
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
using LibRenderer.Abstractions.Drawers;
using LibRenderer.Constants;
using LibRenderer.Implementations;
using LibRenderer.Implementations.UI;
using NLog;
using Microsoft.Extensions.DependencyInjection;

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

    /// <summary>
    /// Bottom UI panel
    /// </summary>
    private ILayer _uiBottomLayer;
    
    #endregion
    
    #region Mouse

    private bool _isDisplayMoving;
    
    private double _oldMouseX;
    private double _oldMouseY;
    
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
        _layers.Add(new GeoTiffLayer("Resources/Gorica.tif"));
        
        // Listening for properties changes to process resize
        PropertyChanged += OnPropertyChangedListener;
        
        // Setting-up input events
        PointerPressed += OnMousePressed;
        PointerReleased += OnMouseReleased;
        PointerMoved += OnMouseMoved;
        PointerWheelChanged += OnWheel;
    }

    private void OnMousePressed(object? sender, PointerPressedEventArgs e)
    {
        _oldMouseX = e.GetCurrentPoint(this).Position.X * _scaling;
        _oldMouseY = e.GetCurrentPoint(this).Position.Y * _scaling;

        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            _isDisplayMoving = true;
        }
    }

    private void OnMouseMoved(object? sender, PointerEventArgs e)
    {
        var newMouseX = e.GetCurrentPoint(this).Position.X * _scaling;
        var newMouseY = e.GetCurrentPoint(this).Position.Y * _scaling;

        if (_isDisplayMoving)
        {
            (_backingImageGeoProvider as DisplayGeoProvider).MoveDisplay(_oldMouseX, _oldMouseY, newMouseX, newMouseY);
        }

        (_uiBottomLayer as UiBottomLayer).Data.MouseLat = _backingImageGeoProvider.YToLat(newMouseY);
        (_uiBottomLayer as UiBottomLayer).Data.MouseLon = _backingImageGeoProvider.XToLon(newMouseX);
        _uiBottomLayer.RegeneratePixelsArray();

        _oldMouseX = newMouseX;
        _oldMouseY = newMouseY;
        
        InvalidateVisual();
    }
    
    private void OnMouseReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            _isDisplayMoving = false;
        }
    }
    
    private void OnWheel(object? sender, PointerWheelEventArgs e)
    {
        var steps = Math.Abs(e.Delta.Y);

        double zoomFactor;
        if (e.Delta.Y > 0)
        {
            zoomFactor = RendererConstants.ZoomInStep;
        }
        else
        {
            zoomFactor = RendererConstants.ZoomOutStep;
        }
        
        (_backingImageGeoProvider as DisplayGeoProvider).Zoom((_backingImageGeoProvider as DisplayGeoProvider).Resolution * zoomFactor, _oldMouseX, _oldMouseY);
        
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
        _backingImageGeoProvider = new DisplayGeoProvider(_viewportWidth, _viewportHeight);

        // Recreating UI
        _uiBottomLayer = new UiBottomLayer(Program.Di.GetService<ITextDrawer>(), _viewportWidth);
    }
    
    /// <summary>
    /// Render the control
    /// </summary>
    public override unsafe void Render(DrawingContext context)
    {
        base.Render(context);
        
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

                    var isPixelExist = layer.GetPixelCoordinates(backingLat, backingLon, out var layerX, out var layerY);
                    if (isPixelExist)
                    {
                        GetPixel(layerPixels, layer.Width, (int)layerX, (int)layerY, out var lp0, out var lp1, out var lp2, out var lp3);
                        
                        var opacity = lp3 / (double)0xFF;

                        _backingArray[backingIndex] = MixBrightness(lp0, _backingArray[backingIndex], opacity);
                        _backingArray[backingIndex + 1] = MixBrightness(lp1, _backingArray[backingIndex + 1], opacity);
                        _backingArray[backingIndex + 2] = MixBrightness(lp2, _backingArray[backingIndex + 2], opacity);
                        _backingArray[backingIndex + 3] = 0xFF;
                    }
                });
            }
        }
        
        // Special layer - bottom UI
        var bottomUiPanelYShift = _viewportHeight - _uiBottomLayer.Height;
        var bottomUiPanelPixels = _uiBottomLayer.GetPixelsArray();
        for (var y = 0; y < _uiBottomLayer.Height; y++)
        {
            Parallel.For(0, _viewportWidth,
            x =>
            {
                var layerIndex = (y * _viewportWidth + x) * 4;
                var backingIndex = ((y + bottomUiPanelYShift) * _viewportWidth + x) * 4;
                
                var opacity = bottomUiPanelPixels[layerIndex + 3] / (double)0xFF;
                
                _backingArray[backingIndex] = MixBrightness(bottomUiPanelPixels[layerIndex], _backingArray[backingIndex], opacity);
                _backingArray[backingIndex + 1] = MixBrightness(bottomUiPanelPixels[layerIndex + 1], _backingArray[backingIndex + 1], opacity);
                _backingArray[backingIndex + 2] = MixBrightness(bottomUiPanelPixels[layerIndex + 2], _backingArray[backingIndex + 2], opacity);
                _backingArray[backingIndex + 3] = 0xFF;
            });
        }

        // Rendering backing image
        fixed (byte* pixels = _backingArray)
        {
            var bitmapToDraw = new Bitmap(PixelFormat.Rgba8888, (nint)pixels, new PixelSize(_viewportWidth, _viewportHeight), new Vector(RendererConstants.DefaultDPI / _scaling, RendererConstants.DefaultDPI / _scaling), _viewportWidth * 4);
            context.DrawImage(bitmapToDraw, new Rect(0, 0, _viewportWidth, _viewportHeight));
        }
    }

    /// <summary>
    /// To avoid heap allocation
    /// </summary>
    private void GetPixel(byte[] pixels, int width, int x, int y, out byte r0, out byte r1, out byte r2, out byte r3)
    {
        var index = (y * width + x) * 4;
        
        r0 = pixels[index + 0];
        r1 = pixels[index + 1];
        r2 = pixels[index + 2];
        r3 = pixels[index + 3];
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
