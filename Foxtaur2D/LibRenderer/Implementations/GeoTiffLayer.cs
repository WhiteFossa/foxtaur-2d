using ImageMagick;
using LibGeo.Abstractions;
using LibGeo.Implementations;
using LibRenderer.Abstractions;
using LibResources.Abstractions.Readers;
using LibResources.Implementations.Readers;

namespace LibRenderer.Implementations;

public class GeoTiffLayer : ILayer
{
    private IGeoTiffReader _geoTiffReader; 
    
    private MagickImage _image;
    private byte[] _pixels;
    
    public int Width { get; }
    public int Height { get; }
    public IGeoProvider GeoProvider { get; }

    public GeoTiffLayer(string path)
    {
        _geoTiffReader = new GeoTiffReader();
        _geoTiffReader.Open(path);

        Width = _geoTiffReader.GetWidth();
        Height = _geoTiffReader.GetHeight();
        
        var pixels = new byte[Width * Height * 4];
        for (var y = 0; y < Height; y++)
        {
            Parallel.For(0, Width,
                x =>
                {
                    var baseIndex = (y * Width + x) * 4;
                    pixels[baseIndex] = (byte)Math.Floor(_geoTiffReader.GetPixel(1, x, y) * 255 + 0.5);
                    pixels[baseIndex + 1] = (byte)Math.Floor(_geoTiffReader.GetPixel(2, x, y) * 255 + 0.5);
                    pixels[baseIndex + 2] = (byte)Math.Floor(_geoTiffReader.GetPixel(3, x, y) * 255 + 0.5);
                    pixels[baseIndex + 3] = (byte)Math.Floor(_geoTiffReader.GetPixel(4, x, y) * 255 + 0.5);
                });
        }

        var readSettings = new MagickReadSettings();
        readSettings.ColorType = ColorType.TrueColorAlpha;
        readSettings.Width = Width;
        readSettings.Height = Height;
        readSettings.Format = MagickFormat.Rgba;
                
        _image = new MagickImage(pixels, readSettings);
        
        GeoProvider = new FlatGeoProvider(Width, Height); // TODO: Replace with real geo provider
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