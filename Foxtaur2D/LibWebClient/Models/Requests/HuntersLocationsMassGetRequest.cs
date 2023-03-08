using System.Text.Json.Serialization;

namespace LibWebClient.Models.Requests;

/// <summary>
/// Parameters for hunters locations mass get
/// </summary>
public class HuntersLocationsMassGetRequest
{
    /// <summary>
    /// List of hunters IDs, for what we need to get data
    /// </summary>
    [JsonPropertyName("huntersIds")]
    public IReadOnlyCollection<Guid> HuntersIds { get; }

    /// <summary>
    /// Get hunters history since this time
    /// </summary>
    [JsonPropertyName("fromTime")]
    public DateTime FromTime { get; }

    public HuntersLocationsMassGetRequest(IReadOnlyCollection<Guid> huntersIds, DateTime fromTime)
    {
        HuntersIds = huntersIds ?? throw new ArgumentNullException(nameof(huntersIds));
        FromTime = fromTime;
    }
}