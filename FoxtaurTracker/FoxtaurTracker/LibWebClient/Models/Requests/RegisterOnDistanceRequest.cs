using System.Text.Json.Serialization;

namespace LibWebClient.Models.Requests;

/// <summary>
/// Request for registration on distance
/// </summary>
public class RegisterOnDistanceRequest
{
    /// <summary>
    /// Register on this distance
    /// </summary>
    [JsonPropertyName("distanceId")]
    public Guid DistanceId { get; }

    public RegisterOnDistanceRequest(Guid distanceId)
    {
        DistanceId = distanceId;
    }
}