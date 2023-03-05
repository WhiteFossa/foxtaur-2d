using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Services.Abstract;

/// <summary>
/// Service to work with maps
/// </summary>
public interface IMapsService
{
    /// <summary>
    /// Get map by Id. Will return null if map doesn't exist
    /// </summary>
    Task<MapDto> GetMapByIdAsync(Guid id);
}