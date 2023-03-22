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
    
    /// <summary>
    /// Get hunters history to this time
    /// </summary>
    [JsonPropertyName("toTime")]
    public DateTime ToTime { get; }

    public HuntersLocationsMassGetRequest(IReadOnlyCollection<Guid> huntersIds, DateTime fromTime, DateTime toTime)
    {
        HuntersIds = huntersIds ?? throw new ArgumentNullException(nameof(huntersIds));

        if (toTime <= fromTime)
        {
            throw new ArgumentOutOfRangeException(nameof(toTime), "To time must be greater than from time.");
        }
        
        FromTime = fromTime;
        ToTime = toTime;
    }
}