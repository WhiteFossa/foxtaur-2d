using LibWebClient.Models;
using LibWebClient.Models.Requests;

namespace LibWebClient.Services.Abstract;

/// <summary>
/// High-level web client
/// </summary>
public interface IWebClient
{
    /// <summary>
    /// Connect to server
    /// </summary>
    Task ConnectAsync();

    /// <summary>
    /// Get server info
    /// </summary>
    Task<ServerInfo> GetServerInfoAsync();
    
    /// <summary>
    /// Register user on server
    /// </summary>
    Task<bool> RegisterUserAsync(RegistrationRequest request);

    /// <summary>
    /// Log in to server
    /// </summary>
    Task<LoginResult> LoginAsync(LoginRequest request);
}