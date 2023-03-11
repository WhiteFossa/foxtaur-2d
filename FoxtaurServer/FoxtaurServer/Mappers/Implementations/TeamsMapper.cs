using FoxtaurServer.Dao.Models;
using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Mappers.Implementations;

public class TeamsMapper : ITeamsMapper
{
    private readonly IColorsMapper _colorsMapper;

    public TeamsMapper(IColorsMapper colorsMapper)
    {
        _colorsMapper = colorsMapper;
    }
    
    public IReadOnlyCollection<TeamDto> Map(IEnumerable<Team> teams)
    {
        if (teams == null)
        {
            return null;
        }

        return teams.Select(t => Map(t)).ToList();
    }

    public TeamDto Map(Team team)
    {
        if (team == null)
        {
            return null;
        }

        return new TeamDto(team.Id, team.Name, _colorsMapper.Map(team.ColorR, team.ColorG, team.ColorB, team.ColorA));
    }

    public Team Map(TeamDto team)
    {
        if (team == null)
        {
            return null;
        }

        return new Team()
        {
            Id = team.Id,
            Name = team.Name,
            ColorA = team.Color.A,
            ColorR = team.Color.R,
            ColorG = team.Color.G,
            ColorB = team.Color.B
        };
    }

    public IReadOnlyCollection<Team> Map(IEnumerable<TeamDto> teams)
    {
        if (teams == null)
        {
            return null;
        }

        return teams.Select(t => Map(t)).ToList();
    }
}