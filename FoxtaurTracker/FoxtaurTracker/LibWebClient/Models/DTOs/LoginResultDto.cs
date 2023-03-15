using System.Text.Json.Serialization;

namespace LibWebClient.Models.DTOs;

/// <summary>
/// Login result
/// </summary>
public class LoginResultDto
{
    /// <summary>
    /// Token
    /// </summary>
    [JsonPropertyName("token")]
    public string Token { get; set; }

    /// <summary>
    /// Token expiration date and time
    /// </summary>
    [JsonPropertyName("expiration")]
    public DateTime ExpirationTime { get; set; }
}