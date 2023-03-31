using LibBusinessLogic.Services.Abstract;
using LibWebClient.Models;

namespace LibBusinessLogic.Services.Implementations;

public class SortingService : ISortingService
{
    public IReadOnlyCollection<Distance> SortDistances(IReadOnlyCollection<Distance> distances)
    {
        return distances
            .OrderByDescending(d => d.FirstHunterStartTime)
            .ThenBy(d => d.Name)
            .ToList();
    }

    public IReadOnlyCollection<Team> SortTeams(IReadOnlyCollection<Team> teams)
    {
        return teams
            .OrderBy(t => t.Name)
            .ToList();
    }

    public IReadOnlyCollection<Hunter> SortHunters(IReadOnlyCollection<Hunter> hunters)
    {
        return hunters
            .OrderBy(h => h.Name)
            .ToList();
    }
}