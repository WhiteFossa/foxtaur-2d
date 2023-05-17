using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api;

public class MapFileDto
{
    /// <summary>
    /// Map file Id
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; }

    /// <summary>
    /// User-friendly file name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; }
    
    /// <summary>
    /// Is map file ready to use?
    /// </summary>
    [JsonPropertyName("isReady")]
    public bool IsReady { get; }

    public MapFileDto
    (
        Guid id,
        string name,
        bool isReady
    )
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }

        Id = id;
        Name = name;
        IsReady = isReady;
    }
}