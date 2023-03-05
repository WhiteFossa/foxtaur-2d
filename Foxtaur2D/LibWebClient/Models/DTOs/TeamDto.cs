using System.Text.Json.Serialization;

namespace LibWebClient.Models.DTOs;

/// <summary>
/// Team
/// </summary>
public class TeamDto
{
    /// <summary>
    /// Team Id
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }
}