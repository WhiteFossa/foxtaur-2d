using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api.Requests;

/// <summary>
/// Request to create map file
/// </summary>
public class CreateMapFileRequest
{
    
    /// <summary>
    /// User-friendly file name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Size, in bytes
    /// </summary>
    [JsonPropertyName("size")]
    public int Size { get; set; }
}