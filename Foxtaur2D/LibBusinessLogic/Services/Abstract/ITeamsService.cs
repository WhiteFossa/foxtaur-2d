using LibWebClient.Models;

namespace LibBusinessLogic.Services.Abstract;

/// <summary>
/// Service to work with teams
/// </summary>
public interface ITeamsService
{
    /// <summary>
    /// Replaces null item (it must be only one) in teams list with "No team" team.
    /// </summary>
    IReadOnlyCollection<Team> InjectTeamlessTeam(IReadOnlyCollection<Team> teams);

    /// <summary>
    /// Search through the hunters and set "No team" team for those, who have no team
    /// </summary>
    IReadOnlyCollection<Hunter> ApplyTeamlessTeamToHunters(IReadOnlyCollection<Hunter> hunters);
}