using System.Text.Json.Serialization;

namespace LibWebClient.Models.DTOs;

/// <summary>
/// Hunter location, it have more specific data than usual location
/// </summary>
public class HunterLocationDto
{
    /// <summary>
    /// Location ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Latitude
    /// </summary>
    [JsonPropertyName("latitude")]
    public double Lat { get; set; }

    /// <summary>
    /// Longitude
    /// </summary>
    [JsonPropertyName("longitude")]
    public double Lon { get; set; }

    /// <summary>
    /// Altitude
    /// </summary>
    [JsonPropertyName("altitude")]
    public double Alt { get; set; }
}