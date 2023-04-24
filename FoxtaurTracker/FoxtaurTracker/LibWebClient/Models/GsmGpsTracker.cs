namespace LibWebClient.Models;

/// <summary>
/// GSM-interfaced GPS tracker
/// </summary>
public class GsmGpsTracker
{
    /// <summary>
    /// Tracker ID
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Tracker IMEI (we use as credentials)
    /// </summary>
    public string Imei { get; }
    
    /// <summary>
    /// Tracker name (may be non-unique)
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// For now tracker is being used by this user
    /// </summary>
    public Guid? UsedBy { get; }

    public GsmGpsTracker
    (
        Guid id,
        string imei,
        string name,
        Guid? usedBy
    )
    {
        if (string.IsNullOrWhiteSpace(imei))
        {
            throw new ArgumentException("IMEI must be not empty!", nameof(imei));
        }
        
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Name must be not empty!", nameof(name));
        }

        Id = id;
        Imei = imei;
        Name = name;
        UsedBy = usedBy;
    }
}