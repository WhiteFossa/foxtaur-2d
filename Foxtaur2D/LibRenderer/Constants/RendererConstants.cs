using Avalonia;
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
    /// UI font path (used in part of UI only)
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

    /// <summary>
    /// Distance border corners radius
    /// </summary>
    public const double DistanceBorderCornersRadius = 10.0;

    /// <summary>
    /// Distance name font size
    /// </summary>
    public const double DistanceNameFontSize = 32;
    
    /// <summary>
    /// Distance name color
    /// </summary>
    public static readonly Color DistanceNameColor = new Color(255, 0, 0, 255);

    #region Start
    
    /// <summary>
    /// Start element color
    /// </summary>
    public static readonly Color StartColor = new Color(255, 255, 0, 0);

    /// <summary>
    /// Radius of outer circle, inside what start triangle is drawn
    /// </summary>
    public const double StartR = 25.0;
    
    /// <summary>
    /// Start pen thickness
    /// </summary>
    public const double StartPenThickness = 3.0;
    
    /// <summary>
    /// Start name font size
    /// </summary>
    public const double StartNameFontSize = 32;
    
    #endregion
    
    #region Foxes
    
    /// <summary>
    /// Fox element color
    /// </summary>
    public static readonly Color FoxColor = new Color(255, 255, 0, 0);
    
    /// <summary>
    /// Fox pen thickness
    /// </summary>
    public const double FoxPenThickness = 3.0;
    
    /// <summary>
    /// Fox radius
    /// </summary>
    public const double FoxRadius = 20.0;
    
    /// <summary>
    /// Fox name font size
    /// </summary>
    public const double FoxNameFontSize = 32;
    
    /// <summary>
    /// Fox description font size
    /// </summary>
    public const double FoxDescriptionFontSize = 20;
    
    #endregion
    
    #region Finish corridor entrance
    
    /// <summary>
    /// Finish corridor entrance element color
    /// </summary>
    public static readonly Color FinishCorridorEntranceColor = new Color(255, 255, 0, 0);
    
    /// <summary>
    /// Finish corridor entrance pen thickness
    /// </summary>
    public const double FinishCorridorEntrancePenThickness = 3.0;
    
    /// <summary>
    /// Finish corridor entrance radius
    /// </summary>
    public const double FinishCorridorEntranceRadius = 25.0;
    
    /// <summary>
    /// Finish corridor entrance name font size
    /// </summary>
    public const double FinishCorridorEntranceNameFontSize = 32;
    
    #endregion

    #region Finish

    /// <summary>
    /// Finish element color
    /// </summary>
    public static readonly Color FinishColor = new Color(255, 255, 0, 0);

    /// <summary>
    /// Finish pen thickness
    /// </summary>
    public const double FinishPenThickness = 3.0;
    
    /// <summary>
    /// Finish outer radius
    /// </summary>
    public const double FinishOuterRadius = 25.0;
    
    /// <summary>
    /// Finish inner radius
    /// </summary>
    public const double FinishInnerRadius = 15.0;
    
    /// <summary>
    /// Finish name font size
    /// </summary>
    public const double FinishNameFontSize = 32;

    #endregion

    #region Linker lines

    /// <summary>
    /// Linker lines color
    /// </summary>
    public static readonly Color LinkerLinesColor = new Color(255, 255, 0, 0);

    /// <summary>
    /// Linker line thickness
    /// </summary>
    public const double LinkerLinesThickness = 3.0;
    
    #endregion

    #region Hunters

    /// <summary>
    /// Hunter marker size in pixels
    /// </summary>
    public static readonly Size HunterMarkerSize = new Size(45, 45);

    /// <summary>
    /// Active point on hunter marker (this point will be displayed in point with hunter coordinates)
    /// </summary>
    public static readonly Point HunterMarkerActivePoint = new Point(22.5, 44);

    /// <summary>
    /// Hunter name font size
    /// </summary>
    public const double HunterNameFontSize = 32;

    /// <summary>
    /// Shift hunter name down to this amount of pixels
    /// </summary>
    public const double HunterNameShiftDown = 15.0;
    
    /// <summary>
    /// Hunter team name font size
    /// </summary>
    public const double HunterTeamNameFontSize = 32;

    /// <summary>
    /// Hunter linker line thickness
    /// </summary>
    public const double HunterLinkerLinesThickness = 3.0;
    
    #endregion

    #endregion
    
    #region Data transmission

    /// <summary>
    /// Color for indicator of hunter data download initialization
    /// </summary>
    public static Color HuntersDataDownloadInitiatedColor = Colors.Yellow;

    /// <summary>
    /// Color for indicator of hunter data download completion
    /// </summary>
    public static Color HuntersDataDownloadCompletedColor = Colors.Green;
    
    /// <summary>
    /// Color for indicator of hunter data download failure
    /// </summary>
    public static Color HuntersDataDownloadFailureColor = Colors.Red;
    
    #endregion
}