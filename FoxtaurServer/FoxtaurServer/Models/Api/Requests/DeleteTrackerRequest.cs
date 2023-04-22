using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api.Requests;

/// <summary>
/// Delete tracker request
/// </summary>
public class DeleteTrackerRequest
{
    /// <summary>
    /// Claim this tracker
    /// </summary>
    [JsonPropertyName("trackerId")]
    public Guid TrackerId { get; set; }
}