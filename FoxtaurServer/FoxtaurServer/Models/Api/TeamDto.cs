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

    public TeamDto(Guid id, string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }

        Id = id;
        Name = name;
    }
}