using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api.Requests;

/// <summary>
/// Claim a tracker as our
/// </summary>
public class ClaimGsmGpsTrackerRequest
{
    /// <summary>
    /// Claim this tracker
    /// </summary>
    [JsonPropertyName("trackerId")]
    public Guid TrackerId { get; set; }
}