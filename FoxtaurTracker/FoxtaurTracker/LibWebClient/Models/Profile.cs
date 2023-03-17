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
    public string FirstName { get; }

    /// <summary>
    /// Middle name
    /// </summary>
    public string MiddleName { get; }

    /// <summary>
    /// Last name
    /// </summary>
    public string LastName { get; }

    /// <summary>
    /// Body sex
    /// </summary>
    public BodySex Sex { get; }

    /// <summary>
    /// Date of birth
    /// </summary>
    public DateTime DateOfBirth { get; }

    /// <summary>
    /// Phone
    /// </summary>
    public string Phone { get; }

    /// <summary>
    /// Team (may be null)
    /// </summary>
    public Team Team { get; }

    /// <summary>
    /// Category
    /// </summary>
    public Category Category { get; }

    /// <summary>
    /// Color
    /// </summary>
    public Color Color { get; }

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