using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api;

/// <summary>
/// Login result
/// </summary>
public class LoginResultDto
{
    /// <summary>
    /// Token
    /// </summary>
    [JsonPropertyName("token")]
    public string Token { get; }

    /// <summary>
    /// Token expiration date and time
    /// </summary>
    [JsonPropertyName("expiration")]
    public DateTime ExpirationTime { get; }

    public LoginResultDto(string token, DateTime expirationTime)
    {
        Token = token;
        ExpirationTime = expirationTime;
    }
}