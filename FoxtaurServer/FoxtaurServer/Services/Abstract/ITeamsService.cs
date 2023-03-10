using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Services.Abstract;

/// <summary>
/// Service to work with teams
/// </summary>
public interface ITeamsService
{
    /// <summary>
    /// Mass get teams by their IDs
    /// </summary>
    Task<IReadOnlyCollection<TeamDto>> MassGetTeamsAsync(IReadOnlyCollection<Guid> teamsIds);
}