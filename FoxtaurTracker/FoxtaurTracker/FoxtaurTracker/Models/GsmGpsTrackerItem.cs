using LibWebClient.Models;

namespace FoxtaurTracker.Models;

/// <summary>
/// GSM-interfaced GPS tracker item
/// </summary>
public class GsmGpsTrackerItem
{
    /// <summary>
    /// Tracker
    /// </summary>
    public GsmGpsTracker Tracker { get; }

    /// <summary>
    /// Index in list
    /// </summary>
    public int Index { get; }
    
    public GsmGpsTrackerItem(GsmGpsTracker tracker, int index)
    {
        Tracker = tracker ?? throw new ArgumentNullException(nameof(tracker));
        Index = index;
    }
}