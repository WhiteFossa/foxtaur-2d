using FoxtaurServer.Constants;
using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Dao.Models;
using FoxtaurServer.Helpers;
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
        return _mapFilesMapper.Map(
            (await _mapFilesDao.GetAllAsync())
            .Where(mf => mf.IsReady));
    }

    public async Task<MapFileDto> CreateNewMapFileAsync(MapFileDto mapFile, int size)
    {
        _ = mapFile ?? throw new ArgumentNullException(nameof(mapFile));

        if (size <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(size), "Size must be positive.");
        }
        
        // Inserting to DB
        var dbMapFile = _mapFilesMapper.Map(mapFile);
        await _mapFilesDao.CreateAsync(dbMapFile);

        // Creating a file
        var fullPath = await GetMapFilePathByIdAsync(dbMapFile.Id);
        
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
        
        return new MapFileDto(dbMapFile.Id, mapFile.Name, dbMapFile.IsReady); // Taking IsReady flag from DB (actually we can hardcode it to false here)
    }

    public async Task UploadMapFilePartAsync(Guid id, int startPosition, byte[] data)
    {
        if (startPosition < 0)
        {
            throw new ArgumentException("Start position must be non-negative.", nameof(startPosition));
        }

        if (data == null)
        {
            throw new ArgumentNullException(nameof(data), "Data must be specified.");
        }
        
        // Do we have such file in DB?
        var dbFile = await _mapFilesDao.GetByIdAsync(id);
        if (dbFile == null)
        {
            throw new ArgumentException("File with given ID doesn't exist.", nameof(id));
        }

        var diskFilePath = await GetMapFilePathByIdAsync(id);

        var fileInfo = new FileInfo(diskFilePath);
        if (fileInfo.Length < startPosition + data.Length)
        {
            // Too long data
            throw new ArgumentException("Data too long.", nameof(data));
        }

        using (var fileStream = File.OpenWrite(diskFilePath))
        {
            fileStream.Seek(startPosition, SeekOrigin.Begin);
            fileStream.Write(data, 0, data.Length);
            fileStream.Flush();
        }
    }

    public async Task MarkMapFileAsReadyAsync(Guid id)
    {
        var dbFile = await _mapFilesDao.GetByIdAsync(id);
        if (dbFile == null)
        {
            throw new ArgumentException("File with given ID doesn't exist.", nameof(id));
        }

        if (dbFile.IsReady)
        {
            throw new InvalidOperationException("This file already marked as ready.");
        }
        
        // Generating hash
        var mapFilePath = await GetMapFilePathByIdAsync(id);
        var content = await File.ReadAllBytesAsync(mapFilePath);
        var hash = FilesHelper.CalculateSHA512(content);

        dbFile.Hash = hash;
        await _mapFilesDao.UpdateAsync(dbFile);
        
        // Marking as ready
        await _mapFilesDao.MarkAsReadyAsync(id);
    }

    public async Task<string> GetMapFilePathByIdAsync(Guid id)
    {
        var pathParts = id
            .ToString()
            .ToLower()
            .Split('-')
            .Select(p => p.Substring(0, GlobalConstants.MapFilesDirectoriesNameLength))
            .ToArray();

        return Path.Combine(GlobalConstants.MapFilesRootPath, Path.Combine(Path.Combine(pathParts), id.ToString().ToLower()));
    }
}