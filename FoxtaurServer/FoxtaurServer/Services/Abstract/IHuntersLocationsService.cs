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
}