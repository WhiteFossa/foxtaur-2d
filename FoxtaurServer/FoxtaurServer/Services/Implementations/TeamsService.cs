using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;

namespace FoxtaurServer.Services.Implementations;

public class TeamsService : ITeamsService
{
    private List<TeamDto> _teams = new List<TeamDto>();

    public TeamsService()
    {
        _teams.Add(new TeamDto(new Guid("AE9EE155-BCDC-44C3-B83F-A4837A3EC443"), "Foxtaurs", new ColorDto(0, 0, 255, 255)));

        _teams.Add(new TeamDto(new Guid("4E44C3DE-4B3A-472B-8289-2072A9F7B49C"), "Fox yiffers", new ColorDto(0, 255, 0, 255)));
    }

    public async Task<IReadOnlyCollection<TeamDto>> MassGetTeamsAsync(IReadOnlyCollection<Guid> teamsIds)
    {
        _ = teamsIds ?? throw new ArgumentNullException(nameof(teamsIds));

        return _teams
            .Where(t => teamsIds.Contains(t.Id))
            .ToList();
    }
}