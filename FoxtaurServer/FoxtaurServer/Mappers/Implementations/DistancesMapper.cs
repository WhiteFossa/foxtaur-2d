using FoxtaurServer.Dao.Models;
using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Mappers.Implementations;

public class DistancesMapper : IDistancesMapper
{
    public IReadOnlyCollection<DistanceDto> Map(IEnumerable<Distance> distances)
    {
        if (distances == null)
        {
            return null;
        }

        return distances.Select(d => Map(d)).ToList();
    }

    public DistanceDto Map(Distance distance)
    {
        if (distance == null)
        {
            return null;
        }

        var orderedFoxesLocationsIds = distance
            .FoxesLocations
            .OrderBy(l => l.Order)
            .Select(l => l.FoxLocation.Id)
            .ToList();

        return new DistanceDto(
            distance.Id,
            distance.Name,
            distance.Map.Id,
            distance.IsActive,
            distance.StartLocation.Id,
            distance.FinishCorridorEntranceLocation.Id,
            distance.FinishLocation.Id,
            orderedFoxesLocationsIds,
            distance.Hunters.Select(h => Guid.Parse(h.Id)).ToList(),
            distance.FirstHunterStartTime,
            distance.CloseTime);
    }

    public Distance Map(DistanceDto distance)
    {
        if (distance == null)
        {
            return null;
        }

        return new Distance()
        {
            Id = distance.Id,
            Name = distance.Name,
            Map = new Map() { Id = distance.MapId },
            IsActive = distance.IsActive,
            StartLocation = new Location() { Id = distance.StartLocationId }, // DAO !MUST! load full entities
            FinishCorridorEntranceLocation = new Location() { Id = distance.FinishCorridorEntranceLocationId },
            FinishLocation = new Location() { Id = distance.FinishLocationId },
            FoxesLocations = new List<DistanceToFoxLocationLinker>(), // !! MAP LINKERS OUTSIDE !!
            Hunters = distance.HuntersIds.Select(hid => new Profile() { Id = hid.ToString() }).ToList(),
            FirstHunterStartTime = distance.FirstHunterStartTime,
            CloseTime = distance.CloseTime
        };
    }

    public IReadOnlyCollection<Distance> Map(IEnumerable<DistanceDto> distances)
    {
        if (distances == null)
        {
            return null;
        }

        return distances.Select(d => Map(d)).ToList();
    }
}