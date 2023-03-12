using System.Text.Json.Serialization;
using FoxtaurServer.Dao.Models.Enums;

namespace FoxtaurServer.Models.Api.Requests;

/// <summary>
/// Location create request
/// </summary>
public class CreateLocationRequest
{
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