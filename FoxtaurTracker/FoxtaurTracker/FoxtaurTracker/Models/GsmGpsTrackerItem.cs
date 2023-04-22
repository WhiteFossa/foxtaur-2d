namespace FoxtaurTracker.Models;

/// <summary>
/// GSM-interfaced GPS tracker item
/// </summary>
public class GsmGpsTrackerItem
{
    /// <summary>
    /// Index in list
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// Tracker name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Tracker IMEI
    /// </summary>
    public string Imei { get; }

    public GsmGpsTrackerItem(int index, string name, string imei)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Tracker name can't be empty.", nameof(name));
        }
        
        if (string.IsNullOrWhiteSpace(imei))
        {
            throw new ArgumentException("Tracker IMEI can't be empty.", nameof(imei));
        }

        Index = index;
        Name = name;
        Imei = imei;
    }
}