using System.Text.Json.Serialization;

namespace LibWebClient.Models.DTOs;

/// <summary>
/// Fox
/// </summary>
public class FoxDto
{
    /// <summary>
    /// Fox ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Fox name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Fox frequency in Hz
    /// </summary>
    [JsonPropertyName("frequency")]
    public double Frequency { get; set; }

    /// <summary>
    /// Fox code
    /// </summary>
    [JsonPropertyName("code")]
    public string Code { get; set; }
}