using FoxtaurServer.Dao.Models;
using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Mappers.Abstract;

/// <summary>
/// Locations mapper
/// </summary>
public interface ILocationsMapper
{
    IReadOnlyCollection<LocationDto> Map(IEnumerable<Location> locations);

    LocationDto Map(Location location);

    Location Map(LocationDto location);

    IReadOnlyCollection<Location> Map(IEnumerable<LocationDto> locations);
}