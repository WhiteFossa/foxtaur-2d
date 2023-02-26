using ImageMagick;
using LibGeo.Models;
using LibRenderer.Abstractions.Drawers;
using LibRenderer.Constants;

namespace LibRenderer.Implementations.Drawers;

public class TextDrawer : ITextDrawer
{
    public ITypeMetric GetTextBounds(MagickImage image, int size, string text)
    {
        _ = image ?? throw new ArgumentNullException(nameof(image));

        image.Settings.FontPointsize = size;
        image.Settings.Font = RendererConstants.UiFontPath;
        return image.FontTypeMetrics(text);
    }

    public void DrawText(MagickImage image, int size, MagickColor color, PlanarPoint origin, string text)
    {
        _ = image ?? throw new ArgumentNullException(nameof(image));
        _ = color ?? throw new ArgumentNullException(nameof(color));
        _ = origin ?? throw new ArgumentNullException(nameof(origin));

        image.Settings.Font = RendererConstants.UiFontPath;
        
        new Drawables()
            .FontPointSize(size)
            .StrokeColor(color)
            .FillColor(color)
            .Text(origin.X, origin.Y, text)
            .Draw(image);
    }
}