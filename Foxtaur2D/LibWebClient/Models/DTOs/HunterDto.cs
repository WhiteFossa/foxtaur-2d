using System.Text.Json.Serialization;

namespace LibWebClient.Models.DTOs;

/// <summary>
/// Hunter
/// </summary>
public class HunterDto
{
    /// <summary>
    /// Hunter Id
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }
    
    /// <summary>
    /// True if hunter is on distance now
    /// </summary>
    [JsonPropertyName("isRunning")]
    public bool IsRunning { get; set; }

    /// <summary>
    /// Team ID, may be null if hunter is teamless
    /// </summary>
    [JsonPropertyName("teamId")]
    public Guid? TeamId { get; set; }
    
    /// <summary>
    /// Color
    /// </summary>
    [JsonPropertyName("color")]
    public ColorDto Color { get; set; }
}