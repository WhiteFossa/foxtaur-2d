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
    
    /// <summary>
    /// UI text color
    /// </summary>
    public static readonly MagickColor UiTextColor = new MagickColor(255, 255, 255, 255);
    
    /// <summary>
    /// UI font path. !! USE RASTER FONTS ONLY !!
    /// </summary>
    public const string UiFontPath = @"Resources/Fonts/helvR-100dpi-34.otb";
    
    /// <summary>
    /// UI font size
    /// </summary>
    public const int UiFontSize = 34;

    /// <summary>
    /// Shift text in the bottom panel to the right to this amount of pixels
    /// </summary>
    public const int BottomUiPanelXShift = 10;

    /// <summary>
    /// Zoom in resolution multiplier
    /// </summary>
    public const double ZoomInStep = 1.1;

    /// <summary>
    /// Zoom out resolution multiplier
    /// </summary>
    public const double ZoomOutStep = 0.9;
}