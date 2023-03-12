using FoxtaurServer.Dao.Models;

namespace FoxtaurServer.Dao.Abstract;

/// <summary>
/// DAO to work with distances
/// </summary>
public interface IDistancesDao
{
    #region Gets

    /// <summary>
    /// Get all existing distances
    /// </summary>
    public Task<IReadOnlyCollection<Distance>> GetAllDistancesAsync();

    /// <summary>
    /// Get distance by given ID. Will return null if distance not found
    /// </summary>
    public Task<Distance> GetDistanceByIdAsync(Guid distanceId);
    
    /// <summary>
    /// Return distance by name or null if not found
    /// </summary>
    Task<Distance> GetDistanceByNameAsync(string name);

    #endregion
    
    #region Create and update

    /// <summary>
    /// Create new distance. ID will be written into distance.Id
    /// </summary>
    Task CreateAsync(Distance distance);

    #endregion
    
}