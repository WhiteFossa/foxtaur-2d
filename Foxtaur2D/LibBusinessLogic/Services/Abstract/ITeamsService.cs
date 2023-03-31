using LibWebClient.Models;

namespace LibBusinessLogic.Services.Abstract;

/// <summary>
/// Service to work with teams
/// </summary>
public interface ITeamsService
{
    /// <summary>
    /// Search through the hunters and set "No team" team for those, who have no team
    /// </summary>
    IReadOnlyCollection<Hunter> ApplyTeamlessTeamToHunters(IReadOnlyCollection<Hunter> hunters);
}