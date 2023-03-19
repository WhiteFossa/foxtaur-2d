using FoxtaurServer.Dao.Models;

namespace FoxtaurServer.Dao.Abstract;

/// <summary>
/// DAO to work with hunters locations
/// </summary>
public interface IHuntersLocationsDao
{
    #region Gets

    /// <summary>
    /// Get hunters locations list by hunter IDs
    /// </summary>
    Task<IReadOnlyCollection<HunterLocation>> GetHuntersLocationsByHuntersIdsAsync(IReadOnlyCollection<Guid> huntersIds, DateTime fromTime);
    
    /// <summary>
    /// Get hunters locations list by hunters locations IDs
    /// </summary>
    Task<IReadOnlyCollection<HunterLocation>> GetHuntersLocationsByIdsAsync(IReadOnlyCollection<Guid> ids, DateTime fromTime);

    #endregion
    
    #region Create and update

    /// <summary>
    /// Mass create new hunters locations
    /// </summary>
    Task MassCreateAsync(IReadOnlyCollection<HunterLocation> huntersLocations);

    /// <summary>
    /// Mass update existing hunters locations
    /// </summary>
    Task MassUpdateAsync(IReadOnlyCollection<HunterLocation> huntersLocations);
    
    #endregion 
}