using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api.Requests;

/// <summary>
/// Request for new user registration
/// </summary>
public class RegistrationRequest
{
    [JsonPropertyName("login")]
    public string Login { get; set; }  
    
    [JsonPropertyName("email")]
    public string Email { get; set; }  
    
    [JsonPropertyName("password")]
    public string Password { get; set; } 
}