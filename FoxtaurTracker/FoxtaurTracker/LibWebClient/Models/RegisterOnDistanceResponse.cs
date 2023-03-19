using System.Text.Json.Serialization;
using LibWebClient.Models.Enums;

namespace LibWebClient.Models;

/// <summary>
/// Response for registration on distance
/// </summary>
public class RegisterOnDistanceResponse
{
    /// <summary>
    /// Registration result
    /// </summary>
    [JsonPropertyName("result")]
    public RegistrationOnDistanceResult Result { get; }

    public RegisterOnDistanceResponse(RegistrationOnDistanceResult result)
    {
        Result = result;
    }
}