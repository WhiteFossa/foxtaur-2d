using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;

namespace FoxtaurServer.Services.Implementations;

public class TeamsService : ITeamsService
{
    private List<TeamDto> _teams = new List<TeamDto>();

    public TeamsService()
    {
        _teams.Add(new TeamDto(new Guid("AE9EE155-BCDC-44C3-B83F-A4837A3EC443"), "Foxtaurs"));

        _teams.Add(new TeamDto(new Guid("4E44C3DE-4B3A-472B-8289-2072A9F7B49C"), "Fox yiffers"));
    }

    public async Task<TeamDto> GetTeamByIdAsync(Guid id)
    {
        return _teams
            .SingleOrDefault(t => t.Id == id);
    }
}