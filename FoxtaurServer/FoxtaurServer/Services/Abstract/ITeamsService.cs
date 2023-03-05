using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Services.Abstract;

/// <summary>
/// Service to work with teams
/// </summary>
public interface ITeamsService
{
    /// <summary>
    /// Get team by ID
    /// Will return null if team didn't found
    /// </summary>
    Task<TeamDto> GetTeamByIdAsync(Guid id);
}