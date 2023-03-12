using FoxtaurServer.Dao.Models;
using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Mappers.Abstract;

/// <summary>
/// Hunters locations
/// </summary>
public interface IHuntersLocationsMapper
{
    IReadOnlyCollection<HunterLocationDto> Map(IEnumerable<HunterLocation> huntersLocations);

    HunterLocationDto Map(HunterLocation hunterLocation);

    HunterLocation Map(HunterLocationDto hunterLocation);

    IReadOnlyCollection<HunterLocation> Map(IEnumerable<HunterLocationDto> huntersLocations);
}