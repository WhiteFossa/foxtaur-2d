using LibWebClient.Models.DTOs;
using LibWebClient.Models.Requests;

namespace LibWebClient.Services.Abstract;

/// <summary>
/// Low-level web client
/// </summary>
public interface IWebClientRaw
{
    /// <summary>
    /// Get information about the server
    /// </summary>
    Task<ServerInfoDto> GetServerInfoAsync();
    
    /// <summary>
    /// Set authentification token for all subsequent queries
    /// </summary>
    Task SetAuthentificationTokenAsync(string token);
    
    /// <summary>
    /// Log in
    /// </summary>
    Task<LoginResultDto> LogInAsync(LoginRequest request);

    /// <summary>
    /// Create map file (empty)
    /// </summary>
    Task<MapFileDto> CreateMapFileAsync(CreateMapFileRequest request);

    /// <summary>
    /// Upload part of map file
    /// </summary>
    Task UploadMapFilePartAsync(UploadMapFilePartRequest request);
}