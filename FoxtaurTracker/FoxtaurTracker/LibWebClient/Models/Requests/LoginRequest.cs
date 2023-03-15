using System.Text.Json.Serialization;

namespace LibWebClient.Models.Requests;

/// <summary>
/// Request for logging in
/// </summary>
public class LoginRequest
{
    [JsonPropertyName("login")]
    public string Login { get; }

    [JsonPropertyName("password")]
    public string Password { get; }

    public LoginRequest(string login, string password)
    {
        if (string.IsNullOrWhiteSpace(login))
        {
            throw new ArgumentException(nameof(login));
        }
        
        if (string.IsNullOrWhiteSpace(password))
        {
            throw new ArgumentException(nameof(password));
        }

        Login = login;
        Password = password;
    }
}