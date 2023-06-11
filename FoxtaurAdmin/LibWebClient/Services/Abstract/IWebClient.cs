using LibWebClient.Models;
using LibWebClient.Models.Requests;

namespace LibWebClient.Services.Abstract;

/// <summary>
/// High-level web client
/// </summary>
public interface IWebClient
{
    /// <summary>
    /// Get server info
    /// </summary>
    Task<ServerInfo> GetServerInfoAsync();
    
    /// <summary>
    /// Log in to server. Stores login and password inside and uses it for session expiration reauthentification
    /// </summary>
    Task<LoginResult> LoginAsync(LoginRequest request);

    /// <summary>
    /// Destroy the session and forget login and password
    /// </summary>
    Task LogoutAsync();

    /// <summary>
    /// Create new, empty, map file
    /// </summary>
    Task<MapFile> CreateMapFileAsync(CreateMapFileRequest request);
    
    /// <summary>
    /// Upload part of map file
    /// </summary>
    Task UploadMapFilePartAsync(UploadMapFilePartRequest request);
    
    /// <summary>
    /// Mark uploaded map file as ready
    /// </summary>
    Task MarkMapFileAsReadyAsync(MarkMapFileAsReadyRequest request);

    /// <summary>
    /// Get all map files
    /// </summary>
    Task<IReadOnlyCollection<MapFile>> GetAllMapFilesAsync();
    
    /// <summary>
    /// Create a map
    /// </summary>
    Task<Map> CreateMapAsync(CreateMapRequest request);
}