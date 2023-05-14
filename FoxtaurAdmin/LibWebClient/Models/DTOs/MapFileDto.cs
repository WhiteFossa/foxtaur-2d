using System.Text.Json.Serialization;

namespace LibWebClient.Models.DTOs;

public class MapFileDto
{
    /// <summary>
    /// Map file Id
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// User-friendly file name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }
}