using Avalonia;
using Avalonia.Media;
using LibGeo.Implementations.Helpers;
using LibRenderer.Constants;

namespace LibRenderer.Implementations.UI;

/// <summary>
/// Class, which can draw UI on context
/// </summary>
public class UiDrawer
{
    /// <summary>
    /// UI data
    /// </summary>
    public UiData Data { get; private set; } = new UiData();

    public void Draw(DrawingContext context, double width, double height, double scalingFactor)
    {
        // Scaled sizes
        var scaledWidth = width / scalingFactor;
        var scaledHeight = height / scalingFactor;

        // Bottom panel
        var bottomPanelHeight = RendererConstants.BottomUiPanelHeight / scalingFactor;
        var bottomPanelRectangleBrush = new SolidColorBrush(RendererConstants.UiPanelsBackgroundColor); 
        
        context.DrawRectangle(bottomPanelRectangleBrush, new Pen(bottomPanelRectangleBrush), new Rect(0, scaledHeight - bottomPanelHeight, width, bottomPanelHeight));
        
        var bottomText = $"{ Data.MouseLat.ToLatString() }, { Data.MouseLon.ToLonString() }";
        var formattedBottomText = new FormattedText(bottomText, Typeface.Default, RendererConstants.UiFontSize, TextAlignment.Left, TextWrapping.NoWrap, new Size(Double.MaxValue, double.MaxValue));

        context.DrawText(new SolidColorBrush(RendererConstants.UiTextColor), new Point(RendererConstants.BottomUiPanelXShift, scaledHeight - (bottomPanelHeight + formattedBottomText.Bounds.Height) / 2.0), formattedBottomText);
    }
}