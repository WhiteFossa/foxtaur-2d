using ImageMagick;
using LibRenderer.Abstractions;

namespace LibRenderer.Implementations;

/// <summary>
/// Layer, containing an image
/// </summary>
public class ImageLayer : ILayer
{
    private MagickImage _image;
    
    public int Width { get; private set; }
    public int Height { get; private set; }

    public ImageLayer(string path)
    {
        _image = new MagickImage(path);

        Width = _image.Width;
        Height = _image.Height;
    }
    
    public byte[] GetPixelsArray()
    {
        return _image.GetPixels().ToByteArray(PixelMapping.RGBA);
    }
}