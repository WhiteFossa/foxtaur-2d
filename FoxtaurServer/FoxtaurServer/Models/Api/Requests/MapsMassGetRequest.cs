using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api.Requests;

/// <summary>
/// Request for mass getting the maps
/// </summary>
public class MapsMassGetRequest
{
    /// <summary>
    /// List of maps IDs to get maps
    /// </summary>
    [JsonPropertyName("mapsIds")]
    public IReadOnlyCollection<Guid> MapsIds { get; set; }
}