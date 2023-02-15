using ImageMagick;
using LibGeo.Abstractions;
using LibGeo.Implementations;
using LibRenderer.Abstractions;

namespace LibRenderer.Implementations;

/// <summary>
/// Layer, containing an image
/// </summary>
public class FlatImageLayer : ILayer
{
    private MagickImage _image;
    
    public int Width { get; private set; }
    public int Height { get; private set; }
    
    public IGeoProvider GeoProvider { get; private set; }

    public FlatImageLayer(string path)
    {
        _image = new MagickImage(path);

        Width = _image.Width;
        Height = _image.Height;

        GeoProvider = new FlatGeoProvider(Width, Height);
    }
    
    public byte[] GetPixelsArray()
    {
        return _image.GetPixels().ToByteArray(PixelMapping.RGBA);
    }
}