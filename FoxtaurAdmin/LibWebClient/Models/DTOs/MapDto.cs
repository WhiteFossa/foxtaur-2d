using System.Text.Json.Serialization;

namespace LibWebClient.Models.DTOs;

/// <summary>
/// Map
/// </summary>
public class MapDto
{
    /// <summary>
    /// Map Id
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

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