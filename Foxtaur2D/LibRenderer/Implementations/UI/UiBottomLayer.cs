using ImageMagick;
using LibGeo.Abstractions;
using LibRenderer.Abstractions;
using LibRenderer.Constants;

namespace LibRenderer.Implementations.UI;

/// <summary>
/// Botton stripe with UI data
/// </summary>
public class UiBottomLayer : ILayer
{
    private MagickImage _image;
    private byte[] _pixels;
    
    public int Width { get; private set; }
    
    public int Height => RendererConstants.BottomUiPanelHeight;
    
    /// <summary>
    /// We have no geo provider for UI
    /// </summary>
    public IGeoProvider GeoProvider => throw new NotImplementedException();
    
    public UiBottomLayer(int width)
    {
        Width = width;

        _image = new MagickImage(RendererConstants.UiPanelsBackgroundColor, Width, RendererConstants.BottomUiPanelHeight);
    }
    
    public void RegeneratePixelsArray()
    {
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
}