using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api.Requests;

/// <summary>
/// Parameters for foxes mass get request
/// </summary>
public class FoxesMassGetRequest
{
    /// <summary>
    /// List of foxes IDs to get foxes data
    /// </summary>
    [JsonPropertyName("foxesIds")]
    public IReadOnlyCollection<Guid> FoxesIds { get; set; }
}