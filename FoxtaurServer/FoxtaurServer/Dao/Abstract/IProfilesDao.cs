using FoxtaurServer.Dao.Models;

namespace FoxtaurServer.Dao.Abstract;

/// <summary>
/// DAO to work with hunters profiles
/// </summary>
public interface IProfilesDao
{
    #region Gets
    
    /// <summary>
    /// Get profiles list by IDs
    /// </summary>
    Task<IReadOnlyCollection<Profile>> GetProfilesAsync(IReadOnlyCollection<string> profilesIds);
    
    #endregion
    
    #region Create and update
    
    /// <summary>
    /// Create new profile. ID will be written into profile.Id if not specified
    /// </summary>
    Task CreateAsync(Profile profile);

    /// <summary>
    /// Overwrite profile with new values
    /// </summary>
    Task UpdateAsync(Profile profile);

    #endregion
}