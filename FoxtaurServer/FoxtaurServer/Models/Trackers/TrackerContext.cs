namespace FoxtaurServer.Models.Trackers;

/// <summary>
/// Context for hardware GPS tracker
/// </summary>
public class TrackerContext
{
    /// <summary>
    /// Tracker IMEI
    /// </summary>
    public string Imei { get; private set; }

    /// <summary>
    /// Set tracker IMEI
    /// </summary>
    public void SetImei(string imei)
    {
        if (string.IsNullOrWhiteSpace(imei))
        {
            throw new ArgumentException("Imei can't be empty", nameof(imei));
        }

        Imei = imei;
    }
}