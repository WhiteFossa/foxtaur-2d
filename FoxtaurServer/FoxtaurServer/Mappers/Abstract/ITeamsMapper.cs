using FoxtaurServer.Dao.Models;
using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Mappers.Abstract;

/// <summary>
/// Teams mapper
/// </summary>
public interface ITeamsMapper
{
    IReadOnlyCollection<TeamDto> Map(IEnumerable<Team> teams);

    TeamDto Map(Team team);

    Team Map(TeamDto team);

    IReadOnlyCollection<Team> Map(IEnumerable<TeamDto> teams);
}