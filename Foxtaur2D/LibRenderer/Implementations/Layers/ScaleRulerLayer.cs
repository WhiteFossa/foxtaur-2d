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
        var distance = 1000000.0;
        var error = double.MaxValue;

        do
        {
            var distanceCurrent = displayGeoProvider.GetPixelsCountByDistance(distance) / scalingFactor;
            var errorCurrent = Math.Abs(RendererConstants.ScaleRulerTargetHalfWidth - distanceCurrent);
            if (errorCurrent < RendererConstants.ScaleRulerAcceptableError)
            {
                // No need to adjust the distance
                break;
            }
            
            double distanceDelta;

            // Hardcoded to avoid too much constants
            if (distance < 10)
            {
                distanceDelta = 0.1;
            }
            else if (distance <= 100)
            {
                distanceDelta = 10;
            }
            else if (distance <= 1000)
            {
                distanceDelta = 100;
            }
            else if (distance <= 10000)
            {
                distanceDelta = 1000;
            }
            else if (distance <= 100000)
            {
                distanceDelta = 10000;
            }
            else
            {
                distanceDelta = 100000;
            }
            
            var distanceLesser = distance - distanceDelta;
            var distanceGreater = distance + distanceDelta;
            
            var lengthLesser = displayGeoProvider.GetPixelsCountByDistance(distanceLesser) / scalingFactor;
            var lengthGreater = displayGeoProvider.GetPixelsCountByDistance(distanceGreater) / scalingFactor;

            var errorLesser = Math.Abs(RendererConstants.ScaleRulerTargetHalfWidth - lengthLesser);
            var errorGreater = Math.Abs(RendererConstants.ScaleRulerTargetHalfWidth - lengthGreater);

            if (errorLesser < errorGreater)
            {
                error = errorLesser;
                distance = distanceLesser;
            }
            else
            {
                error = errorGreater;
                distance = distanceGreater;
            }

        } while (error > RendererConstants.ScaleRulerAcceptableError);

        DrawRuler(context, width, height, scalingFactor, displayGeoProvider, distance);
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
        string formattedDistanceText;
        if (distance >= 1000)
        {
            // Kilometers
            formattedDistanceText = $"{(distance / 1000.0):0.0} km";
        }
        else
        {
            // Meters
            formattedDistanceText = $"{distance:0.0} m";
        }
        
        var text = new FormattedText(formattedDistanceText,
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