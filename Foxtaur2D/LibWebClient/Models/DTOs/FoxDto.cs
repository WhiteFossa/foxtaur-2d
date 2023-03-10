using System.Text.Json.Serialization;
using LibWebClient.Models.Abstract;

namespace LibWebClient.Models.DTOs;

/// <summary>
/// Fox
/// </summary>
public class FoxDto : IIdedDto
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