using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api.Requests;

/// <summary>
/// Request to create a new map
/// </summary>
public class CreateMapRequest
{
    /// <summary>
    /// Name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Bounds - north latitude
    /// </summary>
    [JsonPropertyName("northLat")]
    public double NorthLat { get; set; }

    /// <summary>
    /// Bounds - south latitude
    /// </summary>
    [JsonPropertyName("southLat")]
    public double SouthLat { get; set; }

    /// <summary>
    /// Bounds - east longitude
    /// </summary>
    [JsonPropertyName("eastLon")]
    public double EastLon { get; set; }

    /// <summary>
    /// Bounds - west longitude
    /// </summary>
    [JsonPropertyName("westLon")]
    public double WestLon { get; set; }

    /// <summary>
    /// Map file ID
    /// </summary>
    [JsonPropertyName("fileId")]
    public Guid FileId { get; set; }
}