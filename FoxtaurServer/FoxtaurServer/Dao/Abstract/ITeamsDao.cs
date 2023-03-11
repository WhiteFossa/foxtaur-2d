using FoxtaurServer.Dao.Models;

namespace FoxtaurServer.Dao.Abstract;

/// <summary>
/// DAO to work with teams
/// </summary>
public interface ITeamsDao
{
    #region Gets

    /// <summary>
    /// Get teams list by IDs
    /// </summary>
    Task<IReadOnlyCollection<Team>> GetTeamsAsync(IReadOnlyCollection<Guid> teamsIds);

    /// <summary>
    /// Return team by name or null if not found
    /// </summary>
    Task<Team> GetTeamByNameAsync(string name);
    
    /// <summary>
    /// Get all teams
    /// </summary>
    Task<IReadOnlyCollection<Team>> GetAllTeamsAsync();
    
    #endregion

    #region Create and update

    /// <summary>
    /// Create new team. ID will be written into team.Id
    /// </summary>
    Task CreateAsync(Team team);

    #endregion
}