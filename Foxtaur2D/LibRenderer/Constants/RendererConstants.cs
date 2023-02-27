using Avalonia.Media;
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
    public static readonly Color UiPanelsBackgroundColor = new Color(200, 30, 30, 60);
    
    /// <summary>
    /// UI text color
    /// </summary>
    public static readonly Color UiTextColor = new Color(255, 255, 255, 255);
    
    /// <summary>
    /// UI font path. !! USE RASTER FONTS ONLY !!
    /// </summary>
    public const string UiFontPath = @"Resources/Fonts/NimbusSans-Regular.otf";
    
    /// <summary>
    /// UI font size
    /// </summary>
    public const int UiFontSize = 20;

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

    /// <summary>
    /// Color for "Image is loading" background
    /// </summary>
    public static readonly MagickColor ImageIsLoadingBgColor = new MagickColor(128, 128, 255, 128);

    /// <summary>
    /// Font size for "Image is loading" message
    /// </summary>
    public const int ImageIsLoadingFontSize = 34;
    
    /// <summary>
    /// Color for "Image is loading" message
    /// </summary>
    public static readonly MagickColor ImageIsLoadingFgColor = new MagickColor(255, 255, 255, 255);
    
    #region Distances
    
    /// <summary>
    /// Distance border color
    /// </summary>
    public static readonly Color DistanceBorderColor = new Color(255, 0, 0, 255);

    /// <summary>
    /// Distance border thickness
    /// </summary>
    public const double DistanceBorderThickness = 2.0;

    #endregion
}