using ImageMagick;

namespace LibRenderer.Constants;

/// <summary>
/// Various constants, related to renderer
/// </summary>
public static class RendererConstants
{
    /// <summary>
    /// Monitor DPI at 100% scaling
    /// </summary>
    public const double DefaultDPI = 96.0;

    /// <summary>
    /// Bottom UI stripe height in pixels
    /// </summary>
    public const int BottomUiPanelHeight = 40;
    
    /// <summary>
    /// UI panels background color
    /// </summary>
    public static readonly MagickColor UiPanelsBackgroundColor = new MagickColor(30, 30, 60, 200);
}