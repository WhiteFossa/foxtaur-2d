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
    /// Get all teams
    /// </summary>
    Task<IReadOnlyCollection<Map>> GetAllMapsAsync();
    
    #endregion   
}