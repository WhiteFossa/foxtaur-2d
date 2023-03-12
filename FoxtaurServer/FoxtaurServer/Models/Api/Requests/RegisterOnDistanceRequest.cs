using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api.Requests;

/// <summary>
/// Request for registration on distance
/// </summary>
public class RegisterOnDistanceRequest
{
    /// <summary>
    /// Register on this distance
    /// </summary>
    [JsonPropertyName("distanceId")]
    public Guid DistanceId { get; set; }
}