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

    /// <summary>
    /// Set authentification token for all subsequent queries
    /// </summary>
    Task SetAuthentificationTokenAsync(string token);

    /// <summary>
    /// Mass get hunters profiles
    /// </summary>
    Task<IReadOnlyCollection<Profile>> MassGetProfilesAsync(ProfilesMassGetRequest request);

    /// <summary>
    /// Get information about current user
    /// </summary>
    /// <returns></returns>
    Task<UserInfo> GetCurrentUserInfoAsync();
}