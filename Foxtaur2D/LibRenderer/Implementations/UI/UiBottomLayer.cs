using ImageMagick;
using LibGeo.Abstractions;
using LibGeo.Implementations.Helpers;
using LibGeo.Models;
using LibRenderer.Abstractions;
using LibRenderer.Abstractions.Drawers;
using LibRenderer.Constants;

namespace LibRenderer.Implementations.UI;

/// <summary>
/// Botton stripe with UI data
/// </summary>
public class UiBottomLayer : ILayer
{
    private readonly ITextDrawer _textDrawer;
    
    private MagickImage _image;
    private byte[] _pixels;

    public int Width { get; private set; }
    
    public int Height => RendererConstants.BottomUiPanelHeight;

    /// <summary>
    /// UI data
    /// </summary>
    public UiData Data { get; set; } = new UiData();

    /// <summary>
    /// We have no geo provider for UI
    /// </summary>
    public IGeoProvider GeoProvider => throw new NotImplementedException();
    
    public UiBottomLayer(ITextDrawer textDrawer, int width)
    {
        _textDrawer = textDrawer;
        
        Width = width;
    }
    
    public void RegeneratePixelsArray()
    {
        _image = new MagickImage(RendererConstants.UiPanelsBackgroundColor, Width, RendererConstants.BottomUiPanelHeight);
        
        var text = $"{ Data.MouseLat.ToLatString() }, { Data.MouseLon.ToLonString() }";
        var textSize = _textDrawer.GetTextBounds(_image, RendererConstants.UiFontSize, text);
        var textShiftY = Height - (Height - textSize.TextHeight) / 2.0 + textSize.Descent;
        
        _textDrawer.DrawText(_image,
            RendererConstants.UiFontSize,
            RendererConstants.UiTextColor,
            new PlanarPoint(RendererConstants.BottomUiPanelXShift, textShiftY),
            text);
        
        _pixels = _image.GetPixels().ToByteArray(PixelMapping.RGBA);
    }

    public byte[] GetPixelsArray()
    {
        if (_pixels == null)
        {
            RegeneratePixelsArray();
        }
        
        return _pixels;
    }

    public bool IsPixelExist(double lat, double lon)
    {
        throw new NotImplementedException();
    }
}