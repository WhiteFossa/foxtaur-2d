using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api;

/// <summary>
/// Information about current user
/// </summary>
public class UserInfoDto
{
    /// <summary>
    /// Distance ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; }

    /// <summary>
    /// Login
    /// </summary>
    [JsonPropertyName("login")]
    public string Login { get; }
    
    /// <summary>
    /// Email
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; }

    public UserInfoDto(Guid id, string login, string email)
    {
        if (string.IsNullOrWhiteSpace(login))
        {
            throw new ArgumentException(nameof(login));
        }
        
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException(nameof(email));
        }
        
        Id = id;
        Login = login;
        Email = email;
    }
}