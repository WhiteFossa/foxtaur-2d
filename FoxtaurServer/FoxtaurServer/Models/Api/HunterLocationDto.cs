using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api;

/// <summary>
/// Hunter location, it have more specific data than usual location
/// </summary>
public class HunterLocationDto
{
    /// <summary>
    /// Location ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; }

    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; }

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
    /// Altitude
    /// </summary>
    [JsonPropertyName("altitude")]
    public double Alt { get; }

    public HunterLocationDto(Guid id, DateTime timestamp, double lat, double lon, double alt)
    {
        Id = id;
        Timestamp = timestamp;
        Lat = lat;
        Lon = lon;
        Alt = alt;
    }
}