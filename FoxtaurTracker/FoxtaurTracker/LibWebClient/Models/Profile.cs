using System.Drawing;
using LibWebClient.Models.Enums;

namespace LibWebClient.Models;

/// <summary>
/// User profile
/// </summary>
public class Profile
{
    /// <summary>
    /// Hunter ID
    /// </summary>
    public Guid Id { get; }
    
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
    /// Color
    /// </summary>
    public Color Color { get; set; }

    public Profile(Guid id,
        string firstName,
        string middleName,
        string lastName,
        BodySex sex,
        DateTime dateOfBirth,
        string phone,
        Team team,
        Category category,
        Color color)
    {
        Id = id;
        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        Sex = sex;
        DateOfBirth = dateOfBirth;
        Phone = phone;
        Team = team;
        Category = category;
        Color = color;
    }
}