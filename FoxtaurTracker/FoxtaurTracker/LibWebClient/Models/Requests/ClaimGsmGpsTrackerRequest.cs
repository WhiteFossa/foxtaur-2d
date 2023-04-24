using System.Text.Json.Serialization;

namespace LibWebClient.Models.Requests;

/// <summary>
/// Claim the tracker request
/// </summary>
public class ClaimGsmGpsTrackerRequest
{
    /// <summary>
    /// Claim this tracker
    /// </summary>
    [JsonPropertyName("trackerId")]
    public Guid TrackerId { get; }

    public ClaimGsmGpsTrackerRequest(Guid trackerId)
    {
        TrackerId = trackerId;
    }
}