using FoxtaurServer.Dao.Models;

namespace FoxtaurServer.Dao.Abstract;

/// <summary>
/// DAO to work with locations
/// </summary>
public interface ILocationsDao
{
    #region Gets

    /// <summary>
    /// Get locations list by IDs
    /// </summary>
    Task<IReadOnlyCollection<Location>> GetLocationsAsync(IReadOnlyCollection<Guid> locationsIds);

    /// <summary>
    /// Get all locations
    /// </summary>
    Task<IReadOnlyCollection<Location>> GetAllLocationsAsync();
    
    #endregion
    
    #region Create and update

    /// <summary>
    /// Create new location. ID will be written into location.Id
    /// </summary>
    Task CreateAsync(Location location);

    #endregion 
}