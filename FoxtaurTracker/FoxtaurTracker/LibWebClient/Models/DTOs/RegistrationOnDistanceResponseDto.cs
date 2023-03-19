using System.Text.Json.Serialization;
using LibWebClient.Models.Enums;

namespace LibWebClient.Models.DTOs;

/// <summary>
/// Response for registration on distance
/// </summary>
public class RegistrationOnDistanceResponseDto
{
    /// <summary>
    /// Registration result
    /// </summary>
    [JsonPropertyName("result")]
    public RegistrationOnDistanceResult Result { get; set; }
}