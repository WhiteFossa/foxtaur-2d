using Avalonia;
using Avalonia.Media;
using LibGeo.Abstractions;
using LibRenderer.Abstractions.Layers;
using LibRenderer.Constants;

namespace LibRenderer.Implementations.Layers;

/// <summary>
/// Layer to draw map scale
/// </summary>
public class ScaleRulerLayer : IVectorLayer
{
    public int Order { get; private set; }

    public ScaleRulerLayer(int layerOrder)
    {
        Order = layerOrder;
    }
    
    public void Draw(DrawingContext context, int width, int height, double scalingFactor, IGeoProvider displayGeoProvider)
    {
        DrawRuler(context, width, height, scalingFactor, displayGeoProvider, 1000);
    }

    private void DrawRuler(DrawingContext context, int width, int height, double scalingFactor, IGeoProvider displayGeoProvider, double distance)
    {
        var length = displayGeoProvider.GetPixelsCountByDistance(distance) / scalingFactor;
        
        var rulerYPosition = (height - RendererConstants.ScaleRulerYShiftFromBottom) / scalingFactor;
        var rulerRightPosition = (width - RendererConstants.ScaleRulerXShiftFromRight) / scalingFactor;
        var rulerLeftPosition = rulerRightPosition - length;

        var rulerPen = new Pen(new SolidColorBrush(RendererConstants.ScaleRulerColor), RendererConstants.ScaleRulerLinesThickness);
        
        // Main ruler line
        context.DrawLine(rulerPen,
            new Point(rulerLeftPosition, rulerYPosition),
            new Point(rulerRightPosition, rulerYPosition));

        var rulerSidesHalfHeightScaled = RendererConstants.ScaleRulerSidesHalfHeight / scalingFactor;
        
        // Left border
        context.DrawLine(rulerPen,
            new Point(rulerLeftPosition, rulerYPosition - rulerSidesHalfHeightScaled),
            new Point(rulerLeftPosition, rulerYPosition + rulerSidesHalfHeightScaled));
        
        // Right border
        context.DrawLine(rulerPen,
            new Point(rulerRightPosition, rulerYPosition - rulerSidesHalfHeightScaled),
            new Point(rulerRightPosition, rulerYPosition + rulerSidesHalfHeightScaled));
        
        // Text
        var text = new FormattedText($"{ distance } m",
            Typeface.Default,
            RendererConstants.ScaleRulerTextFontSize,
            TextAlignment.Left,
            TextWrapping.NoWrap,
            new Size(double.MaxValue, double.MaxValue));

        var textLeft = (length - text.Bounds.Width / scalingFactor) / 2.0 + rulerLeftPosition;
        
        context.DrawText(new SolidColorBrush(RendererConstants.ScaleRulerColor),
            new Point(textLeft, rulerYPosition - (text.Bounds.Height - RendererConstants.ScaleRulerTextYShift) / scalingFactor),
            text);
    }
}