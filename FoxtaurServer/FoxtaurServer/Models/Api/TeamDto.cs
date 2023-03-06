using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api;

/// <summary>
/// Team
/// </summary>
public class TeamDto
{
    /// <summary>
    /// Team Id
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; }

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

    public TeamDto(Guid id, string name, ColorDto color)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }

        Id = id;
        Name = name;
        Color = color ?? throw new ArgumentNullException(nameof(color));
    }
}