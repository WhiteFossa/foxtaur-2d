using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Services.Abstract;

/// <summary>
/// Service to work with locations
/// </summary>
public interface ILocationsService
{
    /// <summary>
    /// Mass get locations by their IDs
    /// </summary>
    Task<IReadOnlyCollection<LocationDto>> MassGetLocationsAsync(IReadOnlyCollection<Guid> locationsIds);
    
    /// <summary>
    /// Get all existing locations
    /// </summary>
    Task<IReadOnlyCollection<LocationDto>> GetAllLocationsAsync();
    
    /// <summary>
    /// Create new location. Will return null in case of failure
    /// </summary>
    Task<LocationDto> CreateNewLocationAsync(LocationDto location);
}