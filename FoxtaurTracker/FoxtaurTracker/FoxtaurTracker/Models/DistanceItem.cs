using LibWebClient.Models;

namespace FoxtaurTracker.Models;

/// <summary>
/// Distance item (for dropdown)
/// </summary>
public class DistanceItem
{
    /// <summary>
    /// Distance
    /// </summary>
    public Distance Distance { get; }

    /// <summary>
    /// Index in list
    /// </summary>
    public int Index { get; }
    
    public DistanceItem(Distance distance, int index)
    {
        Distance = distance ?? throw new ArgumentNullException(nameof(distance));
        Index = index;
    }
}