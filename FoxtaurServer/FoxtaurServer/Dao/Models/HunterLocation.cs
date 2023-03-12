using System.ComponentModel.DataAnnotations;

namespace FoxtaurServer.Dao.Models;

/// <summary>
/// Hunter location
/// </summary>
public class HunterLocation
{
    /// <summary>
    /// Location ID
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Timestamp
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Latitude
    /// </summary>
    public double Lat { get; set; }

    /// <summary>
    /// Longitude
    /// </summary>
    public double Lon { get; set; }

    /// <summary>
    /// Altitude
    /// </summary>
    public double Alt { get; set; }

    /// <summary>
    /// Location belongs to this hunter
    /// </summary>
    public Profile Hunter { get; set; }
}