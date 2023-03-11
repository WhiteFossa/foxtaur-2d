using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Services.Abstract;

/// <summary>
/// Service to work with maps
/// </summary>
public interface IMapsService
{
    /// <summary>
    /// Mass get maps by their IDs
    /// </summary>
    Task<IReadOnlyCollection<MapDto>> MassGetMapsAsync(IReadOnlyCollection<Guid> mapsIds);
    
    /// <summary>
    /// Get all maps
    /// </summary>
    Task<IReadOnlyCollection<MapDto>> GetAllMapsAsync();
    
    /// <summary>
    /// Create new map. Will return null in case of failure
    /// </summary>
    Task<MapDto> CreateNewMapAsync(MapDto map);
}