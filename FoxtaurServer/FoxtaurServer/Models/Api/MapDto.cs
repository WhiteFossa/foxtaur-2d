using System.Text.Json.Serialization;
using FoxtaurServer.Models.Api.Enums;

namespace FoxtaurServer.Models.Api;

/// <summary>
/// Map
/// </summary>
public class MapDto
{
    /// <summary>
    /// Map Id
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; }

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
    /// Full URL
    /// </summary>
    [JsonPropertyName("url")]
    public string Url
    {
        get
        {
            return $"http://localhost:5035/api/Files/Download?fileId={ FileId }&type={ DownloadFileType.MapFile }";
        }
    }

    /// <summary>
    /// Map file ID
    /// </summary>
    [JsonPropertyName("fileId")]
    public Guid FileId { get; }

    public MapDto(Guid id,
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
            throw new ArgumentException(nameof(name));
        }

        Id = id;
        Name = name;
        NorthLat = northLat;
        SouthLat = southLat;
        EastLon = eastLon;
        WestLon = westLon;
        FileId = fileId;
    }
}