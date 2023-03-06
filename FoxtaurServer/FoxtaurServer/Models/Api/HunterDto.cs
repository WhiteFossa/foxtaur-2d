using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api;

/// <summary>
/// Hunter
/// </summary>
public class HunterDto
{
    /// <summary>
    /// Hunter Id
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; }

    /// <summary>
    /// Name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; }
    
    /// <summary>
    /// True if hunter is on distance now
    /// </summary>
    [JsonPropertyName("isRunning")]
    public bool IsRunning { get; }

    /// <summary>
    /// Team ID, may be null if hunter is teamless
    /// </summary>
    [JsonPropertyName("teamId")]
    public Guid? TeamId { get; }

    public HunterDto(
        Guid id,
        string name,
        bool isRunning,
        Guid? teamId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }

        Id = id;
        Name = name;
        IsRunning = isRunning;
        TeamId = teamId;
    }
}