using FoxtaurServer.Dao.Models;

namespace FoxtaurServer.Dao.Abstract;

/// <summary>
/// DAO to work with maps
/// </summary>
public interface IMapsDao
{
    #region Gets

    /// <summary>
    /// Get maps list by IDs
    /// </summary>
    Task<IReadOnlyCollection<Map>> GetMapsAsync(IReadOnlyCollection<Guid> mapsIds);

    /// <summary>
    /// Return map by name or null if not found
    /// </summary>
    Task<Map> GetMapByNameAsync(string name);
    
    /// <summary>
    /// Get all maps
    /// </summary>
    Task<IReadOnlyCollection<Map>> GetAllMapsAsync();
    
    #endregion
    
    #region Create and update

    /// <summary>
    /// Create new map. ID will be written into map.Id
    /// </summary>
    Task CreateAsync(Map map);

    #endregion
}