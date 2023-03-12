using FoxtaurServer.Dao.Models;
using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Mappers.Implementations;

public class LocationsMapper : ILocationsMapper
{
    public IReadOnlyCollection<LocationDto> Map(IEnumerable<Location> locations)
    {
        if (locations == null)
        {
            return null;
        }

        return locations.Select(l => Map(l)).ToList();
    }

    public LocationDto Map(Location location)
    {
        if (location == null)
        {
            return null;
        }

        return new LocationDto(location.Id,
            location.Name,
            location.Type,
            location.Lat,
            location.Lon,
            location.Fox?.Id);
    }

    public Location Map(LocationDto location)
    {
        if (location == null)
        {
            return null;
        }

        return new Location()
        {
            Id = location.Id,
            Name = location.Name,
            Type = location.Type,
            Fox = location.FoxId.HasValue ? new Fox() { Id = location.FoxId.Value } : null, // WARNING: Simplified fox - only ID
            Lat = location.Lat,
            Lon = location.Lon
        };
    }

    public IReadOnlyCollection<Location> Map(IEnumerable<LocationDto> locations)
    {
        if (locations == null)
        {
            return null;
        }

        return locations.Select(l => Map(l)).ToList();
    }
}