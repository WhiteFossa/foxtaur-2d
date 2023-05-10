using FoxtaurServer.Constants;
using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Dao.Models;
using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;

namespace FoxtaurServer.Services.Implementations;

public class MapFilesService : IMapFilesService
{
    private readonly IMapFilesDao _mapFilesDao;
    private readonly IMapFilesMapper _mapFilesMapper;

    public MapFilesService
    (
        IMapFilesDao mapFilesDao,
        IMapFilesMapper mapFilesMapper
    )
    {
        _mapFilesDao = mapFilesDao;
        _mapFilesMapper = mapFilesMapper;
    }
    
    public async Task<IReadOnlyCollection<MapFileDto>> GetAllMapFilesAsync()
    {
        return _mapFilesMapper.Map(await _mapFilesDao.GetAllAsync());
    }

    public async Task<MapFileDto> CreateNewMapFileAsync(MapFileDto mapFile, int size)
    {
        _ = mapFile ?? throw new ArgumentNullException(nameof(mapFile));

        if (size <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(size), "Size must be positive.");
        }

        var mapFileId = Guid.NewGuid();
        
        // Inserting to DB
        var dbMapFile = _mapFilesMapper.Map(mapFile);
        dbMapFile.Id = Guid.NewGuid();
        await _mapFilesDao.CreateAsync(dbMapFile);

        // Creating a file
        var fullPath = GenerateMapFilePath(dbMapFile.Id);
        
        var directoryPath = Path.GetDirectoryName(fullPath);
        Directory.CreateDirectory(directoryPath);
        
        // Preallocating the file
        var buffer = new byte[size];
        using (var stream = new MemoryStream(buffer))
        {
            using (var fileStream = File.Create(fullPath))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
            }
        }
        
        return new MapFileDto(dbMapFile.Id, mapFile.Name);
    }

    private string GenerateMapFilePath(Guid mapFileId)
    {
        var pathParts = mapFileId
            .ToString()
            .ToLower()
            .Split('-')
            .Select(p => p.Substring(0, GlobalConstants.MapFilesDirectoriesNameLength))
            .ToArray();

        return Path.Combine(GlobalConstants.MapFilesRootPath, Path.Combine(Path.Combine(pathParts), mapFileId.ToString().ToLower()));
    }
}