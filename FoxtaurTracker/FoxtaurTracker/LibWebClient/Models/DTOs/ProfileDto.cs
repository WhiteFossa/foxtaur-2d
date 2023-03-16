using System.Text.Json.Serialization;
using LibWebClient.Models.Enums;

namespace LibWebClient.Models.DTOs;

/// <summary>
/// Hunter profile
/// </summary>
public class ProfileDto
{
    /// <summary>
    /// Hunter ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    
    /// <summary>
    /// First name
    /// </summary>
    [JsonPropertyName("firstName")]
    public string FirstName { get; set; }

    /// <summary>
    /// Middle name
    /// </summary>
    [JsonPropertyName("middleName")]
    public string MiddleName { get; set; }

    /// <summary>
    /// Last name
    /// </summary>
    [JsonPropertyName("lastName")]
    public string LastName { get; set; }

    /// <summary>
    /// Body sex
    /// </summary>
    [JsonPropertyName("sex")]
    public BodySex Sex { get; set; }

    /// <summary>
    /// Date of birth
    /// </summary>
    [JsonPropertyName("dateOfBirth")]
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Phone
    /// </summary>
    [JsonPropertyName("phone")]
    public string Phone { get; set; }

    /// <summary>
    /// Team (may be null)
    /// </summary>
    [JsonPropertyName("team")]
    public TeamDto Team { get; set; }

    /// <summary>
    /// Category
    /// </summary>
    [JsonPropertyName("category")]
    public Category Category { get; set; }

    /// <summary>
    /// Color
    /// </summary>
    [JsonPropertyName("color")]
    public ColorDto Color { get; set; }
}