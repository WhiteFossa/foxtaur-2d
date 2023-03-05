using System.Text.Json.Serialization;
using LibWebClient.Enums;

namespace LibWebClient.Models.DTOs;

/// <summary>
/// Location
/// </summary>
public class LocationDto
{
    /// <summary>
    /// Location ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Location name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Location type
    /// </summary>
    [JsonPropertyName("type")]
    public LocationType Type { get; set; }

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
    /// Id of fox, installed in this location.
    /// Valid only if Type == Fox, otherwise must be null.
    /// </summary>
    [JsonPropertyName("foxId")]
    public Guid? FoxId { get; set; }
}