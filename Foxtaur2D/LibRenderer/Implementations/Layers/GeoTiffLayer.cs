using ImageMagick;
using LibGeo.Models;
using LibRenderer.Abstractions.Drawers;
using LibRenderer.Abstractions.Layers;
using LibRenderer.Constants;
using LibResources.Abstractions.Readers;
using LibResources.Implementations.Readers;

namespace LibRenderer.Implementations.Layers;

public class GeoTiffLayer : IRasterLayer
{
    private readonly IGeoTiffReader _geoTiffReader;
    private readonly ITextDrawer _textDrawer;
    
    private MagickImage _image;
    private byte[] _pixels;
    
    public int Width { get; }
    public int Height { get; }
    
    public GeoTiffLayer(string path, ITextDrawer textDrawer)
    {
        _textDrawer = textDrawer;
        
        _geoTiffReader = new GeoTiffReader();
        _geoTiffReader.Open(path);

        Width = _geoTiffReader.GetWidth();
        Height = _geoTiffReader.GetHeight();

        // "Image is not loaded yet..." message
        _image = new MagickImage(RendererConstants.ImageIsLoadingBgColor, Width, Height);
        
        var message = "Image is not loaded yet...";
        var messageTextSize = _textDrawer.GetTextBounds(_image, RendererConstants.ImageIsLoadingFontSize, message);

        var messageShiftX = (Width - messageTextSize.TextWidth) / 2.0;
        var messageShiftY = (Height - messageTextSize.TextHeight) / 2.0;
        
        _textDrawer.DrawText(_image,
            RendererConstants.ImageIsLoadingFontSize,
            RendererConstants.ImageIsLoadingFgColor,
            new PlanarPoint(messageShiftX, messageShiftY),
            message);
    }

    /// <summary>
    /// Load actual image
    /// </summary>
    public void Load()
    {
        _image.Dispose();
        
        _geoTiffReader.LoadRasterData();
        
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
        
        RegeneratePixelsArray();
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
        _geoTiffReader.GetPixelCoordsByGeoCoords(lat, lon, out x, out y);
        
        if (x < 0
            ||
            x >= Width
            ||
            y < 0
            ||
            y >= Height)
        {
            return false;
        }

        return true;
    }
}