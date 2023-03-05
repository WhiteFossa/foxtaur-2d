using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Services.Abstract;

/// <summary>
/// Service to work with hunters
/// </summary>
public interface IHuntersService
{
    /// <summary>
    /// Get hunter by ID
    /// Will return null if hunter with given ID doesn't exist
    /// </summary>
    Task<HunterDto> GetHunterByIdAsync(Guid id);
}