using LibWebClient.Models.DTOs;

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
}