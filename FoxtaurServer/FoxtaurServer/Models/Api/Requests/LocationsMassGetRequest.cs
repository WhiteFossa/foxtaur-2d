using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api.Requests;

/// <summary>
/// Request for mass getting locations
/// </summary>
public class LocationsMassGetRequest
{
    /// <summary>
    /// List of locations IDs to get locations
    /// </summary>
    [JsonPropertyName("locationsIds")]
    public IReadOnlyCollection<Guid> LocationsIds { get; set; }
}