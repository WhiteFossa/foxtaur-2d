using System.Text.Json.Serialization;
using FoxtaurServer.Dao.Models;
using FoxtaurServer.Dao.Models.Enums;

namespace FoxtaurServer.Models.Api;

/// <summary>
/// Hunter profile
/// </summary>
public class ProfileDto
{
    /// <summary>
    /// Hunter ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; }
    
    /// <summary>
    /// First name
    /// </summary>
    [JsonPropertyName("firstName")]
    public string FirstName { get; }

    /// <summary>
    /// Middle name
    /// </summary>
    [JsonPropertyName("middleName")]
    public string MiddleName { get; }

    /// <summary>
    /// Last name
    /// </summary>
    [JsonPropertyName("lastName")]
    public string LastName { get; }

    /// <summary>
    /// Body sex
    /// </summary>
    [JsonPropertyName("sex")]
    public BodySex Sex { get; }

    /// <summary>
    /// Date of birth
    /// </summary>
    [JsonPropertyName("dateOfBirth")]
    public DateTime DateOfBirth { get; }

    /// <summary>
    /// Phone
    /// </summary>
    [JsonPropertyName("phone")]
    public string Phone { get; }

    /// <summary>
    /// Team (may be null)
    /// </summary>
    [JsonPropertyName("team")]
    public TeamDto Team { get; }

    /// <summary>
    /// Category
    /// </summary>
    [JsonPropertyName("category")]
    public Category Category { get; }

    /// <summary>
    /// Color
    /// </summary>
    [JsonPropertyName("color")]
    public ColorDto Color { get; }

    public ProfileDto(Guid id,
        string firstName,
        string middleName,
        string lastName,
        BodySex sex,
        DateTime dateOfBirth,
        string phone,
        TeamDto team,
        Category category,
        ColorDto color)
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