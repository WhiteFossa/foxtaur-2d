using System.Text.Json.Serialization;

namespace LibWebClient.Models.Requests;

/// <summary>
/// Request for mass getting the hunters
/// </summary>
public class HuntersMassGetRequest
{
    /// <summary>
    /// List of hunters IDs to get hunters
    /// </summary>
    [JsonPropertyName("huntersIds")]
    public IReadOnlyCollection<Guid> HuntersIds { get; }

    public HuntersMassGetRequest(IReadOnlyCollection<Guid> huntersIds)
    {
        HuntersIds = huntersIds ?? throw new ArgumentNullException(nameof(huntersIds));
    }
}