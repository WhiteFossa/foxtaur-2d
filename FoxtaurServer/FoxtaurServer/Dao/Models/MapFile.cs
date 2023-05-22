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
    /// Is map file ready to use?
    /// </summary>
    public bool IsReady { get; set; }

    /// <summary>
    /// File contents hash (to use as ETag)
    /// </summary>
    public string Hash { get; set; }

    /// <summary>
    /// File creation time
    /// </summary>
    public DateTime CreationTime { get; set; }
}