using FoxtaurServer.Dao.Models;
using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Mappers.Implementations;

public class HuntersLocationsMapper : IHuntersLocationsMapper
{
    public IReadOnlyCollection<HunterLocationDto> Map(IEnumerable<HunterLocation> huntersLocations)
    {
        if (huntersLocations == null)
        {
            return null;
        }

        return huntersLocations.Select(hl => Map(hl)).ToList();
    }

    public HunterLocationDto Map(HunterLocation hunterLocation)
    {
        if (hunterLocation == null)
        {
            return null;
        }

        return new HunterLocationDto(
            hunterLocation.Id,
            hunterLocation.Timestamp,
            hunterLocation.Lat,
            hunterLocation.Lon,
            hunterLocation.Alt);
    }

    public HunterLocation Map(HunterLocationDto hunterLocation)
    {
        if (hunterLocation == null)
        {
            return null;
        }

        return new HunterLocation()
        {
            Id = hunterLocation.Id,
            Timestamp = hunterLocation.Timestamp,
            Lat = hunterLocation.Lat,
            Lon = hunterLocation.Lon,
            Alt = hunterLocation.Alt,
            Hunter = null // IMPORTANT: Must be filled outside
        };
    }

    public IReadOnlyCollection<HunterLocation> Map(IEnumerable<HunterLocationDto> huntersLocations)
    {
        if (huntersLocations == null)
        {
            return null;
        }

        return huntersLocations.Select(hl => Map(hl)).ToList();
    }
}