using System.ComponentModel.DataAnnotations;

namespace FoxtaurServer.Dao.Models;

/// <summary>
/// Ordered linker for linking distances and fox locations
/// </summary>
public class DistanceToFoxLocationLinker
{
    /// <summary>
    /// Primary key
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Distance
    /// </summary>
    public Distance Distance { get; set; }

    /// <summary>
    /// Fox location
    /// </summary>
    public Location FoxLocation { get; set; }

    /// <summary>
    /// Fox order for given location
    /// </summary>
    public int Order { get; set; }
}