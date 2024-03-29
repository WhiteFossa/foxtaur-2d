using FoxtaurServer.Dao.Models;
using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Mappers.Implementations;

public class MapFilesMapper : IMapFilesMapper
{
    public IReadOnlyCollection<MapFileDto> Map(IEnumerable<MapFile> mapFiles)
    {
        if (mapFiles == null)
        {
            return null;
        }

        return mapFiles.Select(mf => Map(mf)).ToList();
    }

    public MapFileDto Map(MapFile mapFile)
    {
        if (mapFile == null)
        {
            return null;
        }

        return new MapFileDto
            (
                mapFile.Id,
                mapFile.Name,
                mapFile.IsReady
            );
    }

    public MapFile Map(MapFileDto mapFile)
    {
        if (mapFile == null)
        {
            return null;
        }

        return new MapFile()
        {
            Id = mapFile.Id,
            Name = mapFile.Name,
            IsReady = false // This field is not mappable to DB direction
        };
    }

    public IReadOnlyCollection<MapFile> Map(IEnumerable<MapFileDto> mapFiles)
    {
        if (mapFiles == null)
        {
            return null;
        }

        return mapFiles.Select(mf => Map(mf)).ToList();
    }
}