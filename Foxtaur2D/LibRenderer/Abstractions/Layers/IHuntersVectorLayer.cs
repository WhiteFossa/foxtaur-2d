using Avalonia.Media;
using LibGeo.Abstractions;
using LibWebClient.Models;

namespace LibRenderer.Abstractions.Layers;

public interface IHuntersVectorLayer : IVectorLayer
{
    /// <summary>
    /// Draw vector layer
    /// </summary>
    /// <param name="context">Drawing context</param>
    /// <param name="width">Display width</param>
    /// <param name="height">Display height</param>
    /// <param name="scalingFactor">Display scaling factor</param>
    /// <param name="displayGeoProvider">Display geo provider</param>
    /// <param name="hunters">Hunters to draw</param>
    void Draw(DrawingContext context, int width, int height, double scalingFactor, IGeoProvider displayGeoProvider, IReadOnlyCollection<Hunter> hunters);
}