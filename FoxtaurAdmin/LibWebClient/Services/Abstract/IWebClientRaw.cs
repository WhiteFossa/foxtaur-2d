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

    /// <summary>
    /// Mark uploaded map file as ready
    /// </summary>
    Task MarkMapFileAsReadyAsync(MarkMapFileAsReadyRequest request);

    /// <summary>
    /// Get all existing map files
    /// </summary>
    Task<IReadOnlyCollection<MapFileDto>> GetAllMapFilesAsync();
    
    /// <summary>
    /// Create a map
    /// </summary>
    Task<MapDto> CreateMapAsync(CreateMapRequest request);
}