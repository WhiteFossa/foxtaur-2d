using System.ComponentModel.DataAnnotations;

namespace FoxtaurServer.Dao.Models;

/// <summary>
/// Map DB model
/// </summary>
public class Map
{
    /// <summary>
    /// Map Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Bounds - north latitude
    /// </summary>
    public double NorthLat { get; set; }

    /// <summary>
    /// Bounds - south latitude
    /// </summary>
    public double SouthLat { get; set; }

    /// <summary>
    /// Bounds - east longitude
    /// </summary>
    public double EastLon { get; set; }

    /// <summary>
    /// Bounds - west longitude
    /// </summary>
    public double WestLon { get; set; }

    /// <summary>
    /// Full URL
    /// </summary>
    public string Url { get; set; }

    /// <summary>
    /// File with this map
    /// </summary>
    public MapFile File { get; set; }
}