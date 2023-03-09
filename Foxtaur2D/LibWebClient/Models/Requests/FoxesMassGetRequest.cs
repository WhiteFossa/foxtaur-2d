using System.Text.Json.Serialization;

namespace LibWebClient.Models.Requests;

/// <summary>
/// Parameters for foxes mass get request
/// </summary>
public class FoxesMassGetRequest
{
    /// <summary>
    /// List of foxes IDs to get foxes data
    /// </summary>
    [JsonPropertyName("foxesIds")]
    public IReadOnlyCollection<Guid> FoxesIds { get; }

    public FoxesMassGetRequest(IReadOnlyCollection<Guid> foxesIds)
    {
        FoxesIds = foxesIds ?? throw new ArgumentNullException(nameof(foxesIds));
    }
}