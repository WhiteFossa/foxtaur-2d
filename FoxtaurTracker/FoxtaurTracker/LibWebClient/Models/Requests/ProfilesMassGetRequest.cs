using System.Text.Json.Serialization;

namespace LibWebClient.Models.Requests;

/// <summary>
/// Request for mass querying hunters profiles
/// </summary>
public class ProfilesMassGetRequest
{
    /// <summary>
    /// List of hunters IDs to get profiles
    /// </summary>
    [JsonPropertyName("huntersIds")]
    public IReadOnlyCollection<Guid> HuntersIds { get; }

    public ProfilesMassGetRequest(IReadOnlyCollection<Guid> huntersIds)
    {
        HuntersIds = huntersIds ?? throw new ArgumentNullException(nameof(huntersIds));
    }
}