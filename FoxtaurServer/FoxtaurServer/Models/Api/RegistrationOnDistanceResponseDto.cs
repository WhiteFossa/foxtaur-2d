using System.Text.Json.Serialization;
using FoxtaurServer.Models.Api.Enums;

namespace FoxtaurServer.Models.Api;

/// <summary>
/// Response for registration on distance
/// </summary>
public class RegistrationOnDistanceResponseDto
{
    /// <summary>
    /// Registration result
    /// </summary>
    [JsonPropertyName("result")]
    public RegistrationOnDistanceResult Result { get; }

    public RegistrationOnDistanceResponseDto(RegistrationOnDistanceResult result)
    {
        Result = result;
    }
}