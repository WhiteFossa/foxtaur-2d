using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api.Requests;

/// <summary>
/// Request for mass getting the hunters
/// </summary>
public class HuntersMassGetRequest
{
    /// <summary>
    /// List of hunters IDs to get hunters
    /// </summary>
    [JsonPropertyName("huntersIds")]
    public IReadOnlyCollection<Guid> HuntersIds { get; set; }
}