using FoxtaurServer.Dao.Models;
using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Mappers.Abstract;

/// <summary>
/// Mapper to work with distances
/// </summary>
public interface IDistancesMapper
{
    IReadOnlyCollection<DistanceDto> Map(IEnumerable<Distance> distances);

    DistanceDto Map(Distance distance);

    Distance Map(DistanceDto distance);

    IReadOnlyCollection<Distance> Map(IEnumerable<DistanceDto> distances);
}