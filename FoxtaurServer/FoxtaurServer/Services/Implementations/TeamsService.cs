using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;

namespace FoxtaurServer.Services.Implementations;

public class TeamsService : ITeamsService
{
    private readonly ITeamsDao _teamsDao;

    private readonly ITeamsMapper _teamsMapper;
    
    public TeamsService(ITeamsDao teamsDao,
        ITeamsMapper teamsMapper)
    {
        _teamsDao = teamsDao;
        _teamsMapper = teamsMapper;
    }

    public async Task<IReadOnlyCollection<TeamDto>> MassGetTeamsAsync(IReadOnlyCollection<Guid> teamsIds)
    {
        _ = teamsIds ?? throw new ArgumentNullException(nameof(teamsIds));

        return _teamsMapper.Map(await _teamsDao.GetTeamsAsync(teamsIds));
    }

    public async Task<TeamDto> CreateNewTeamAsync(TeamDto team)
    {
        _ = team ?? throw new ArgumentNullException(nameof(team));

        // Do we have team with such name?
        var existingTeam = await _teamsDao.GetTeamByNameAsync(team.Name);
        if (existingTeam != null)
        {
            return null;
        }
        
        var mappedTeam = _teamsMapper.Map(team);
        
        await _teamsDao.CreateAsync(mappedTeam);

        return new TeamDto(mappedTeam.Id, team.Name, team.Color);
    }
}