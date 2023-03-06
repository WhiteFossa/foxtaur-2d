using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api;

/// <summary>
/// Just a color
/// </summary>
public class ColorDto
{
    /// <summary>
    /// Red
    /// </summary>
    [JsonPropertyName("r")]
    public byte R { get; }
    
    /// <summary>
    /// Green
    /// </summary>
    [JsonPropertyName("g")]
    public byte G { get; }
    
    /// <summary>
    /// Blue
    /// </summary>
    [JsonPropertyName("b")]
    public byte B { get; }
    
    /// <summary>
    /// Alpha
    /// </summary>
    [JsonPropertyName("a")]
    public byte A { get; }

    public ColorDto(byte r, byte g, byte b, byte a)
    {
        R = r;
        G = g;
        B = b;
        A = a;
    }
}