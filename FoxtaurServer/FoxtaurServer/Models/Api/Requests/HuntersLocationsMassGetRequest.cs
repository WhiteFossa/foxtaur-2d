using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api.Requests;

/// <summary>
/// Parameters for hunters locations mass get
/// </summary>
public class HuntersLocationsMassGetRequest
{
    /// <summary>
    /// List of hunters IDs, for what we need to get data
    /// </summary>
    [JsonPropertyName("huntersIds")]
    public List<Guid> HuntersIds { get; set; }

    /// <summary>
    /// Get hunters history since this time
    /// </summary>
    [JsonPropertyName("fromTime")]
    public DateTime FromTime { get; set; }
    
    /// <summary>
    /// Get hunters history to this time
    /// </summary>
    [JsonPropertyName("toTime")]
    public DateTime ToTime { get; set; }
}