using Avalonia;
using Avalonia.Media;
using LibGeo.Abstractions;
using LibRenderer.Abstractions.Layers;
using LibRenderer.Constants;
using LibWebClient.Models;

namespace LibRenderer.Implementations.Layers;

public class DistanceLayer : IVectorLayer
{
    private readonly Distance _distance;
    
    public DistanceLayer(Distance distanceModel)
    {
        _distance = distanceModel ?? throw new ArgumentNullException(nameof(distanceModel));
    }
    
    public void Draw(DrawingContext context, int width, int height, double scalingFactor, IGeoProvider displayGeoProvider)
    {
        // Distance bounds
        var topY = displayGeoProvider.LatToY(_distance.Map.NorthLat) / scalingFactor;
        var bottomY = displayGeoProvider.LatToY(_distance.Map.SouthLat) / scalingFactor;
        var leftX = displayGeoProvider.LonToX(_distance.Map.WestLon) / scalingFactor;
        var rightX = displayGeoProvider.LonToX(_distance.Map.EastLon) / scalingFactor;
        
        context.DrawRectangle(new SolidColorBrush(new Color(0, 255, 255, 255)), // Always transparent, no need for a constant
            new Pen(new SolidColorBrush(RendererConstants.DistanceBorderColor), RendererConstants.DistanceBorderThickness),
            new Rect(new Point(leftX, topY), new Point(rightX, bottomY)));
        
        // Distance name
        var formattedDistanceName = new FormattedText(_distance.Name,
            Typeface.Default,
            RendererConstants.DistanceNameFontSize,
            TextAlignment.Left,
            TextWrapping.NoWrap,
            new Size(double.MaxValue, double.MaxValue));
        
        context.DrawText(new SolidColorBrush(RendererConstants.DistanceNameColor),
            new Point(leftX, topY - formattedDistanceName.Bounds.Height),
            formattedDistanceName);
    }
}