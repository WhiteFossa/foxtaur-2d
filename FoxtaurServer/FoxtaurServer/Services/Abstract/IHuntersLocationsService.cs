using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Services.Abstract;

/// <summary>
/// Service to work with hunters locations
/// </summary>
public interface IHuntersLocationsService
{
    /// <summary>
    /// Get hunter location by ID
    /// Will return null if location wasn't found
    /// </summary>
    Task<HunterLocationDto> GetHunterLocationByIdAsync(Guid id);

    /// <summary>
    /// Get last hunter location for given hunter
    /// Will return null if hunter ID is incorrect
    /// </summary>
    Task<HunterLocationDto> GetLastHunterLocationByHunterIdAsync(Guid id);

    /// <summary>
    /// Get hunter locations history (ordered) by hunter ID starting from fromTime
    /// Will return null if hunter ID is incorrect
    /// </summary>
    Task<IReadOnlyCollection<HunterLocationDto>> GetHunterLocationsHistoryByHunterIdAsync(Guid id, DateTime fromTime);

    /// <summary>
    /// Get hunters locations dictionary, where key is hunter id from huntersIds, and value is locations history (ordered) starting from fromTime
    /// </summary>
    Task<Dictionary<Guid, IReadOnlyCollection<HunterLocationDto>>> MassGetHuntersLocationsAsync(IReadOnlyCollection<Guid> huntersIds, DateTime fromTime);
}