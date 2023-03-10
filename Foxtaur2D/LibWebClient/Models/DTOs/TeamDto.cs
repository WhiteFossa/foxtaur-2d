using System.Text.Json.Serialization;
using LibWebClient.Models.Abstract;

namespace LibWebClient.Models.DTOs;

/// <summary>
/// Team
/// </summary>
public class TeamDto : IIdedDto
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
    
    /// <summary>
    /// Color
    /// </summary>
    [JsonPropertyName("color")]
    public ColorDto Color { get; set; }
}