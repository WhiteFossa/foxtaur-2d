using FoxtaurServer.Dao.Models;
using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Mappers.Abstract;

/// <summary>
/// Maps mapper
/// </summary>
public interface IMapsMapper
{
    IReadOnlyCollection<MapDto> Map(IEnumerable<Map> maps);

    MapDto Map(Map map);

    Map Map(MapDto map);

    IReadOnlyCollection<Map> Map(IEnumerable<MapDto> maps);
}