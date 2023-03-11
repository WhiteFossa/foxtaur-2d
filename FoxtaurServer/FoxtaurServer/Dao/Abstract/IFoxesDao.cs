using FoxtaurServer.Dao.Models;

namespace FoxtaurServer.Dao.Abstract;

/// <summary>
/// DAO to work with foxes
/// </summary>
public interface IFoxesDao
{
    #region Gets

    /// <summary>
    /// Get foxes list by IDs
    /// </summary>
    Task<IReadOnlyCollection<Fox>> GetFoxesAsync(IReadOnlyCollection<Guid> foxesIds);

    /// <summary>
    /// Return fox by name or null if not found
    /// </summary>
    Task<Fox> GetFoxByNameAsync(string name);
    
    /// <summary>
    /// Get all foxes
    /// </summary>
    Task<IReadOnlyCollection<Fox>> GetAllFoxesAsync();
    
    #endregion
    
    #region Create and update

    /// <summary>
    /// Create new fox. ID will be written into fox.Id
    /// </summary>
    Task CreateAsync(Fox fox);

    #endregion
}