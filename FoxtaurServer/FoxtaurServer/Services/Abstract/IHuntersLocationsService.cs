using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Services.Abstract;

/// <summary>
/// Service to work with hunters locations
/// </summary>
public interface IHuntersLocationsService
{
    /// <summary>
    /// Get hunters locations dictionary, where key is hunter id from huntersIds, and value is locations history (ordered) starting from fromTime
    /// </summary>
    Task<Dictionary<Guid, IReadOnlyCollection<HunterLocationDto>>> MassGetHuntersLocationsAsync(IReadOnlyCollection<Guid> huntersIds, DateTime fromTime);
    
    /// <summary>
    /// Mass add locations to given hunter
    /// </summary>
    Task<Dictionary<Guid, IReadOnlyCollection<HunterLocationDto>>> MassCreateHuntersLocationsAsync(IReadOnlyCollection<HunterLocationDto> huntersLocations, Guid hunterId);
}