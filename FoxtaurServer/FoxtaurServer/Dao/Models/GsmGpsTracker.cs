using System.ComponentModel.DataAnnotations;

namespace FoxtaurServer.Dao.Models;

/// <summary>
/// GSM-interfaced GPS-tracker
/// </summary>
public class GsmGpsTracker
{
    /// <summary>
    /// Tracker ID
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Tracker IMEI (we use it as credentials)
    /// </summary>
    public string Imei { get; set; }

    /// <summary>
    /// Tracker name (may be non-unique)
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// For now tracker is being used by this user
    /// </summary>
    public Profile UsedBy { get; set; }
}