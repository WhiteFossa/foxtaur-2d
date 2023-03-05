using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Services.Abstract;

/// <summary>
/// Service to work with locations
/// </summary>
public interface ILocationsService
{
    /// <summary>
    /// Get location by id
    /// Will return null if location with given ID doesn't exist
    /// </summary>
    Task<LocationDto> GetLocationByIdAsync(Guid id);
}