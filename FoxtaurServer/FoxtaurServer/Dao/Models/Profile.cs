using System.ComponentModel.DataAnnotations;
using FoxtaurServer.Dao.Models.Enums;

namespace FoxtaurServer.Dao.Models;

/// <summary>
/// Hunter profile
/// </summary>
public class Profile
{
    /// <summary>
    /// Primary key (user ID)
    /// </summary>
    [Key]
    public string Id { get; set; }

    /// <summary>
    /// First name
    /// </summary>
    public string FirstName { get; set; }

    /// <summary>
    /// Middle name
    /// </summary>
    public string MiddleName { get; set; }

    /// <summary>
    /// Last name
    /// </summary>
    public string LastName { get; set; }

    /// <summary>
    /// Body sex
    /// </summary>
    public BodySex Sex { get; set; }

    /// <summary>
    /// Date of birth
    /// </summary>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Phone
    /// </summary>
    public string Phone { get; set; }

    /// <summary>
    /// Team (may be null)
    /// </summary>
    public Team Team { get; set; }

    /// <summary>
    /// Category
    /// </summary>
    public Category Category { get; set; }
    
    /// <summary>
    /// A-component of hunter color
    /// </summary>
    public byte ColorA { get; set; }
    
    /// <summary>
    /// R-component of hunter color
    /// </summary>
    public byte ColorR { get; set; }
    
    /// <summary>
    /// G-component of hunter color
    /// </summary>
    public byte ColorG { get; set; }
    
    /// <summary>
    /// B-component of hunter color
    /// </summary>
    public byte ColorB { get; set; }

    /// <summary>
    /// List of distances, to what hunter was registered
    /// </summary>
    public IList<Distance> ParticipatedInDistances { get; set; }
}