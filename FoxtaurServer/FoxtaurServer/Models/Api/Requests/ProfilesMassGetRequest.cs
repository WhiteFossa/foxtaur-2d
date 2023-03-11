using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api.Requests;

/// <summary>
/// Request for mass querying hunters profiles
/// </summary>
public class ProfilesMassGetRequest
{
    /// <summary>
    /// List of hunters IDs to get profiles
    /// </summary>
    [JsonPropertyName("huntersIds")]
    public IReadOnlyCollection<Guid> HuntersIds { get; set; }
}