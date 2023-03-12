using System.Text.Json.Serialization;
using FoxtaurServer.Dao.Models.Enums;

namespace FoxtaurServer.Models.Api;

/// <summary>
/// Location
/// </summary>
public class LocationDto
{
    /// <summary>
    /// Location ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; }

    /// <summary>
    /// Location name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; }

    /// <summary>
    /// Location type
    /// </summary>
    [JsonPropertyName("type")]
    public LocationType Type { get; }

    /// <summary>
    /// Latitude
    /// </summary>
    [JsonPropertyName("latitude")]
    public double Lat { get; }

    /// <summary>
    /// Longitude
    /// </summary>
    [JsonPropertyName("longitude")]
    public double Lon { get; }

    /// <summary>
    /// Id of fox, installed in this location.
    /// Valid only if Type == Fox, otherwise must be null.
    /// </summary>
    [JsonPropertyName("foxId")]
    public Guid? FoxId { get; }

    public LocationDto(
        Guid id,
        string name,
        LocationType type,
        double lat,
        double lon,
        Guid? foxId)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }

        if (type == LocationType.Fox && !foxId.HasValue)
        {
            throw new ArgumentException(nameof(foxId));
        }

        if (type != LocationType.Fox && foxId.HasValue)
        {
            throw new ArgumentException(nameof(foxId));
        }

        Id = id;
        Name = name;
        Type = type;
        Lat = lat;
        Lon = lon;
        FoxId = foxId;
    }
}