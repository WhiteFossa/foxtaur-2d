using System.ComponentModel.DataAnnotations;

namespace FoxtaurServer.Dao.Models;

/// <summary>
/// Team
/// </summary>
public class Team
{
    /// <summary>
    /// Primari key
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// A-component of team color
    /// </summary>
    public byte ColorA { get; set; }
    
    /// <summary>
    /// R-component of team color
    /// </summary>
    public byte ColorR { get; set; }
    
    /// <summary>
    /// G-component of team color
    /// </summary>
    public byte ColorG { get; set; }
    
    /// <summary>
    /// B-component of team color
    /// </summary>
    public byte ColorB { get; set; }
}