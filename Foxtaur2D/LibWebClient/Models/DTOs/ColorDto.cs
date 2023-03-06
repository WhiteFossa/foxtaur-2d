using System.Text.Json.Serialization;

namespace LibWebClient.Models.DTOs;

/// <summary>
/// Just a color
/// </summary>
public class ColorDto
{
    /// <summary>
    /// Red
    /// </summary>
    [JsonPropertyName("r")]
    public byte R { get; set; }
    
    /// <summary>
    /// Green
    /// </summary>
    [JsonPropertyName("g")]
    public byte G { get; set; }
    
    /// <summary>
    /// Blue
    /// </summary>
    [JsonPropertyName("b")]
    public byte B { get; set; }
    
    /// <summary>
    /// Alpha
    /// </summary>
    [JsonPropertyName("a")]
    public byte A { get; set; }
}