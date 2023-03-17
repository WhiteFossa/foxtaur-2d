using System.Text.Json.Serialization;
using LibWebClient.Models.DTOs;
using LibWebClient.Models.Enums;

namespace LibWebClient.Models.Requests;

/// <summary>
/// Request to update profile
/// </summary>
public class ProfileUpdateRequest
{
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
    [JsonPropertyName("teamId")]
    public Guid? TeamId { get; }

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

    public ProfileUpdateRequest(
        string firstName,
        string middleName,
        string lastName,
        BodySex sex,
        DateTime dateOfBirth,
        string phone,
        Guid? teamId,
        Category category,
        ColorDto color)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException(nameof(firstName));
        }
        
        if (string.IsNullOrWhiteSpace(middleName))
        {
            throw new ArgumentException(nameof(middleName));
        }
        
        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException(nameof(lastName));
        }
        
        if (string.IsNullOrWhiteSpace(phone))
        {
            throw new ArgumentException(nameof(phone));
        }

        FirstName = firstName;
        MiddleName = middleName;
        LastName = lastName;
        Sex = sex;
        DateOfBirth = dateOfBirth;
        Phone = phone;
        TeamId = teamId;
        Category = category;
        Color = color;
    }
}