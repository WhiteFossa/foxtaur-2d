using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Visuals.Media.Imaging;
using LibGeo.Abstractions;
using LibRenderer.Abstractions.Layers;
using LibRenderer.Constants;
using LibWebClient.Models;

namespace LibRenderer.Implementations.Layers;

/// <summary>
/// Layer to draw hunters
/// </summary>
public class HuntersLayer : IVectorLayer
{
    /// <summary>
    /// Hunters to display
    /// </summary>
    private IReadOnlyCollection<Hunter> _hunters;
    
    /// <summary>
    /// Bitmap with hunter marker
    /// </summary>
    private Bitmap _hunterMarker;

    public HuntersLayer(IReadOnlyCollection<Hunter> hunters)
    {
        _hunters = hunters ?? throw new ArgumentNullException(nameof(hunters));

        _hunterMarker = new Bitmap(@"Resources/Sprites/hunter_marker.png");
    }
    
    public void Draw(DrawingContext context, int width, int height, double scalingFactor, IGeoProvider displayGeoProvider)
    {
        foreach (var hunter in _hunters)
        {
            DrawHunter(hunter, context, scalingFactor, displayGeoProvider);
        }
    }

    /// <summary>
    /// Draw one hunter
    /// </summary>
    private void DrawHunter(Hunter hunter, DrawingContext context, double scalingFactor, IGeoProvider displayGeoProvider)
    {
        var lastKnownLocation = hunter.LocationsHistory.Last();
        
        var hunterX = displayGeoProvider.LonToX(lastKnownLocation.Lon) / scalingFactor;
        var hunterY = displayGeoProvider.LatToY(lastKnownLocation.Lat) / scalingFactor;
        
        // Marker position
        var markerPosition = new Point(hunterX, hunterY) - RendererConstants.HunterMarkerActivePoint;
        
        // Marker
        context.DrawImage(_hunterMarker, new Rect(new Point(0, 0), _hunterMarker.Size), new Rect(markerPosition, RendererConstants.HunterMarkerSize), BitmapInterpolationMode.HighQuality);
        
        // Name
        var formattedName = new FormattedText(hunter.Name,
            new Typeface(Typeface.Default.FontFamily, FontStyle.Normal, FontWeight.Bold),
            RendererConstants.HunterNameFontSize,
            TextAlignment.Left,
            TextWrapping.NoWrap,
            new Size(double.MaxValue, double.MaxValue));
        
        // Team
        var formattedTeamName = new FormattedText(hunter.Team.Name,
            Typeface.Default,
            RendererConstants.HunterTeamNameFontSize,
            TextAlignment.Left,
            TextWrapping.NoWrap,
            new Size(double.MaxValue, double.MaxValue));
        
        context.DrawText(new SolidColorBrush(hunter.Color),
            new Point(hunterX - formattedName.Bounds.Width / 2.0, markerPosition.Y - formattedName.Bounds.Height - formattedTeamName.Bounds.Height + RendererConstants.HunterNameShiftDown),
            formattedName);
        
        context.DrawText(new SolidColorBrush(hunter.Team.Color),
            new Point(hunterX - formattedTeamName.Bounds.Width / 2.0, markerPosition.Y - formattedTeamName.Bounds.Height),
            formattedTeamName);
        
        // Movements history
        var locationsHistoryAsList = hunter.LocationsHistory.ToList();
        for (var mhi = 0; mhi < locationsHistoryAsList.Count() - 1; mhi++)
        {
            DrawLinkerLine(locationsHistoryAsList[mhi], locationsHistoryAsList[mhi + 1], hunter.Color, context, scalingFactor, displayGeoProvider);
        }
    }
    
    /// <summary>
    /// Draw linker line between two hunter locations
    /// </summary>
    private void DrawLinkerLine(HunterLocation begin, HunterLocation end, Color color, DrawingContext context, double scalingFactor, IGeoProvider displayGeoProvider)
    {
        var beginX = displayGeoProvider.LonToX(begin.Lon) / scalingFactor;
        var beginY = displayGeoProvider.LatToY(begin.Lat) / scalingFactor;
        
        var endX = displayGeoProvider.LonToX(end.Lon) / scalingFactor;
        var endY = displayGeoProvider.LatToY(end.Lat) / scalingFactor;
        
        context.DrawLine(new Pen(new SolidColorBrush(color), RendererConstants.HunterLinkerLinesThickness),
            new Point(beginX, beginY),
            new Point(endX, endY));
    }
}