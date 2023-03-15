using System.Text.Json.Serialization;

namespace LibWebClient.Models.Requests;

/// <summary>
/// Request for new user registration
/// </summary>
public class RegistrationRequest
{
    [JsonPropertyName("login")]
    public string Login { get; }  
    
    [JsonPropertyName("email")]
    public string Email { get; }  
    
    [JsonPropertyName("password")]
    public string Password { get; }

    public RegistrationRequest(string login, string email, string password)
    {
        if (string.IsNullOrEmpty(login))
        {
            throw new ArgumentException(nameof(login));
        }
        
        if (string.IsNullOrEmpty(email))
        {
            throw new ArgumentException(nameof(email));
        }
        
        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException(nameof(password));
        }

        Login = login;
        Email = email;
        Password = password;
    }
}