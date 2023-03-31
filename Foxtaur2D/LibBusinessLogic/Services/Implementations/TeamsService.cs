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