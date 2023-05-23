using FoxtaurServer.Dao.Models;
using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;

namespace FoxtaurServer.Mappers.Implementations;

public class MapsMapper : IMapsMapper
{
    public IReadOnlyCollection<MapDto> Map(IEnumerable<Map> maps)
    {
        if (maps == null)
        {
            return null;
        }

        return maps.Select(m => Map(m)).ToList();
    }

    public MapDto Map(Map map)
    {
        if (map == null)
        {
            return null;
        }

        return new MapDto(map.Id,
            map.Name,
            map.NorthLat,
            map.SouthLat,
            map.EastLon,
            map.WestLon,
            map.File.Id);
    }

    public Map Map(MapDto map)
    {
        if (map == null)
        {
            return null;
        }

        return new Map()
        {
            Id = map.Id,
            Name = map.Name,
            NorthLat = map.NorthLat,
            SouthLat = map.SouthLat,
            EastLon = map.EastLon,
            WestLon = map.WestLon,
            File = new MapFile() { Id = map.FileId} // DAO must load all other fields by itself
        };
    }

    public IReadOnlyCollection<Map> Map(IEnumerable<MapDto> maps)
    {
        if (maps == null)
        {
            return null;
        }

        return maps.Select(m => Map(m)).ToList();
    }
}