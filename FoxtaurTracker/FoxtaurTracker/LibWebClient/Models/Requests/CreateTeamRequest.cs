using System.Text.Json.Serialization;
using LibWebClient.Models.DTOs;

namespace LibWebClient.Models.Requests;

/// <summary>
/// Request for creating new team
/// </summary>
public class CreateTeamRequest
{
    /// <summary>
    /// Name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; }

    /// <summary>
    /// Color
    /// </summary>
    [JsonPropertyName("color")]
    public ColorDto Color { get; }

    public CreateTeamRequest(string name, ColorDto color)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }

        Name = name;
        Color = color;
    }
}