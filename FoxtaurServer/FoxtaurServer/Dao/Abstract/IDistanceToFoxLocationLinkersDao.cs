using FoxtaurServer.Dao.Models;

namespace FoxtaurServer.Dao.Abstract;

/// <summary>
/// DAO to work with Distance-to-FoxLocation linkers
/// </summary>
public interface IDistanceToFoxLocationLinkersDao
{
    #region Create and update

    /// <summary>
    /// Mass create new hunters locations
    /// </summary>
    Task MassCreateAsync(IReadOnlyCollection<DistanceToFoxLocationLinker> linkers);
    
    #endregion 
}