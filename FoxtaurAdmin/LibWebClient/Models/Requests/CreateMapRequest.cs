using System.Text.Json.Serialization;

namespace LibWebClient.Models.Requests;

/// <summary>
/// Request to create a new map
/// </summary>
public class CreateMapRequest
{
    /// <summary>
    /// Name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; }

    /// <summary>
    /// Bounds - north latitude
    /// </summary>
    [JsonPropertyName("northLat")]
    public double NorthLat { get; }

    /// <summary>
    /// Bounds - south latitude
    /// </summary>
    [JsonPropertyName("southLat")]
    public double SouthLat { get; }

    /// <summary>
    /// Bounds - east longitude
    /// </summary>
    [JsonPropertyName("eastLon")]
    public double EastLon { get; }

    /// <summary>
    /// Bounds - west longitude
    /// </summary>
    [JsonPropertyName("westLon")]
    public double WestLon { get; }

    /// <summary>
    /// Map file ID
    /// </summary>
    [JsonPropertyName("fileId")]
    public Guid FileId { get; }

    public CreateMapRequest
    (
        string name,
        double northLat,
        double southLat,
        double eastLon,
        double westLon,
        Guid fileId
    )
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Map name must be populated.", nameof(name));
        }
        Name = name;
        
        NorthLat = northLat;
        SouthLat = southLat;
        EastLon = eastLon;
        WestLon = westLon;
        FileId = fileId;
    }
}