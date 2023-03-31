using Avalonia.Media;
using LibBusinessLogic.Constants;
using LibBusinessLogic.Services.Abstract;
using LibWebClient.Models;

namespace LibBusinessLogic.Services.Implementations;

public class TeamsService : ITeamsService
{
    private readonly Team _teamlessTeam;
    
    public TeamsService()
    {
        _teamlessTeam = new Team
        (
            BusinessLogicConstants.TeamlessTeamId,
            BusinessLogicConstants.TeamlessTeamName,
            BusinessLogicConstants.TeamlessTeamColor
        );
    }
    
    public IReadOnlyCollection<Team> InjectTeamlessTeam(IReadOnlyCollection<Team> teams)
    {
        _ = teams ?? throw new ArgumentNullException(nameof(teams));

        if (teams.Count(t => t == null) > 1)
        {
            throw new ArgumentException("Only one team can be null", nameof(teams));
        }

        return teams
            .Select(t =>
            {
                if (t != null)
                {
                    return t;
                }

                return _teamlessTeam;
            })
            .ToList();
    }

    public IReadOnlyCollection<Hunter> ApplyTeamlessTeamToHunters(IReadOnlyCollection<Hunter> hunters)
    {
        _ = hunters ?? throw new ArgumentNullException();

        return hunters
            .Select(h =>
            {
                if (h.Team != null)
                {
                    return h;
                }

                return new Hunter
                (
                    h.Id,
                    h.Name,
                    h.IsRunning,
                    _teamlessTeam,
                    h.LocationsHistory,
                    h.Color
                );
            })
            .ToList();
    }
}