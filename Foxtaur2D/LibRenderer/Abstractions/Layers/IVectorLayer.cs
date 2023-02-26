using Avalonia.Media;
using LibGeo.Abstractions;

namespace LibRenderer.Abstractions.Layers;

/// <summary>
/// Vector layer
/// </summary>
public interface IVectorLayer : ILayer
{
    /// <summary>
    /// Draw vector layer
    /// </summary>
    /// <param name="context">Drawing context</param>
    /// <param name="width">Display width</param>
    /// <param name="height">Display height</param>
    /// <param name="scalingFactor">Display scaling factor</param>
    /// <param name="displayGeoProvider">Display geo provider</param>
    void Draw(DrawingContext context, int width, int height, double scalingFactor, IGeoProvider displayGeoProvider);
}