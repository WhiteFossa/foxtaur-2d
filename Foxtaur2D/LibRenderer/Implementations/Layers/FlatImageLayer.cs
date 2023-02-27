using ImageMagick;
using LibGeo.Abstractions;
using LibGeo.Implementations;
using LibRenderer.Abstractions.Layers;

namespace LibRenderer.Implementations.Layers;

/// <summary>
/// Layer, containing an image
/// </summary>
public class FlatImageLayer : IRasterLayer
{
    private MagickImage _image;
    private byte[] _pixels;
    private IGeoProvider _geoProvider;
    
    public int Width { get; private set; }
    public int Height { get; private set; }
    
    public FlatImageLayer(string path)
    {
        _image = new MagickImage(path);

        Width = _image.Width;
        Height = _image.Height;

        _geoProvider = new FlatGeoProvider(Width, Height);
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

    public bool GetPixelCoordinates(double lat, double lon, out double x, out double y)
    {
        x = _geoProvider.LonToX(lon);
        y = _geoProvider.LatToY(lat);

        return true;
    }

    public bool IsReady()
    {
        return true;
    }
}