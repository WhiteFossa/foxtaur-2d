using FoxtaurServer.Dao.Models;
using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Mappers.Abstract;

/// <summary>
/// Map files mapper
/// </summary>
public interface IMapFilesMapper
{
    IReadOnlyCollection<MapFileDto> Map(IEnumerable<MapFile> mapFiles);

    MapFileDto Map(MapFile mapFile);

    MapFile Map(MapFileDto mapFile);

    IReadOnlyCollection<MapFile> Map(IEnumerable<MapFileDto> mapFiles);
}