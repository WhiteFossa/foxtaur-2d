using System.ComponentModel.DataAnnotations;

namespace FoxtaurServer.Dao.Models;

/// <summary>
/// File with map
/// </summary>
public class MapFile
{
    /// <summary>
    /// Map file Id
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// User-friendly file name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// File local path (absolute)
    /// </summary>
    public string LocalPath { get; set; }
}