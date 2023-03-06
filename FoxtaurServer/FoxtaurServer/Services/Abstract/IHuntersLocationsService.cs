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
    Task<HunterLocationDto> GetLastHunterLocationByHunterId(Guid id);

    /// <summary>
    /// Get hunter locations history by hunter ID.
    /// Will return null if hunter ID is incorrect
    /// </summary>
    Task<IReadOnlyCollection<HunterLocationDto>> GetHunterLocationsHistoryByHunterId(Guid id);
}