using LibBusinessLogic.Services.Abstract;
using LibWebClient.Models;

namespace LibBusinessLogic.Services.Implementations;

public class DistancesService : IDistancesService
{
    private readonly ITeamsService _teamsService;
    private readonly ISortingService _sortingService;

    public DistancesService
    (
        ITeamsService teamsService,
        ISortingService sortingService
    )
    {
        _teamsService = teamsService;
        _sortingService = sortingService;
    }
    
    public Distance ProcessRawDistance(Distance distance)
    {
        var hunters = _teamsService.ApplyTeamlessTeamToHunters(distance.Hunters);
        hunters = _sortingService.SortHunters(hunters);
        
        distance = new Distance
        (
            distance.Id,
            distance.Name,
            distance.Map,
            distance.IsActive,
            distance.StartLocation,
            distance.FinishCorridorEntranceLocation,
            distance.FinishLocation,
            distance.Foxes,
            hunters,
            distance.FirstHunterStartTime,
            distance.CloseTime
        );

        return distance;
    }
}