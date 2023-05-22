using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Models;
using FoxtaurServer.Models.Api.Enums;
using FoxtaurServer.Services.Abstract;

namespace FoxtaurServer.Services.Implementations;

public class FilesService : IFilesService
{
    private readonly IMapFilesDao _mapFilesDao;
    private readonly IMapFilesService _mapFilesService;

    public FilesService
    (
        IMapFilesDao mapFilesDao,
        IMapFilesService mapFilesService
    )
    {
        _mapFilesDao = mapFilesDao;
        _mapFilesService = mapFilesService;
    }
    
    public Task<FileToDownload> GetFileToDownloadAsync(Guid fileId, DownloadFileType fileType)
    {
        switch (fileType)
        {
            case DownloadFileType.MapFile:
                return GetMapFileToDownloadAsync(fileId);
            
            default:
                throw new ArgumentException("Unsupported file type!", nameof(fileType));
        }
    }

    private async Task<FileToDownload> GetMapFileToDownloadAsync(Guid mapFileId)
    {
        // Do we have such file?
        var mapFile = await _mapFilesDao.GetByIdAsync(mapFileId);
        if (mapFile == null)
        {
            return new FileToDownload(false, null, DateTime.MinValue, string.Empty);
        }

        var mapFilePath = await _mapFilesService.GetMapFilePathByIdAsync(mapFile.Id);
        var content = await File.ReadAllBytesAsync(mapFilePath);
        
        return new FileToDownload(true, content, DateTime.UtcNow, "yiffyuff");
    }
}