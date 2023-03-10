using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api.Requests;

/// <summary>
/// Request for logging in
/// </summary>
public class LoginRequest
{
    [JsonPropertyName("login")]
    public string Login { get; set; }

    [JsonPropertyName("password")]
    public string Password { get; set; } 
}