using ImageMagick;
using LibGeo.Models;

namespace LibRenderer.Abstractions.Drawers;

/// <summary>
/// Draws texts on images
/// </summary>
public interface ITextDrawer
{
    /// <summary>
    /// Get width and height of given text
    /// </summary>
    ITypeMetric GetTextBounds(MagickImage image, int size, string text);

    /// <summary>
    /// Draw given text on an image
    /// </summary>
    void DrawText(MagickImage image, int size, MagickColor color, PlanarPoint origin, string text);
}