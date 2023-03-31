using LibWebClient.Models;

namespace LibBusinessLogic.Services.Abstract;

/// <summary>
/// Service for sorting various data which came from server
/// </summary>
public interface ISortingService
{
    /// <summary>
    /// Sorts distances
    /// </summary>
    IReadOnlyCollection<Distance> SortDistances(IReadOnlyCollection<Distance> distances);

    /// <summary>
    /// Sorts teams
    /// </summary>
    IReadOnlyCollection<Team> SortTeams(IReadOnlyCollection<Team> teams);

    /// <summary>
    /// Sort hunters
    /// </summary>
    /// <param name="hunters"></param>
    /// <returns></returns>
    IReadOnlyCollection<Hunter> SortHunters(IReadOnlyCollection<Hunter> hunters);
}