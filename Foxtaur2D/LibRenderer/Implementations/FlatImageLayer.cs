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
    private byte[] _pixels;
    
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

    public bool IsPixelExist(double lat, double lon)
    {
        return true;
    }
}