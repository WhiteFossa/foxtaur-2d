using FoxtaurServer.Models;
using FoxtaurServer.Models.Api.Enums;

namespace FoxtaurServer.Services.Abstract;

/// <summary>
/// Service to work with files
/// </summary>
public interface IFilesService
{
    /// <summary>
    /// Reads file from storage (if file exist) and returns it
    /// </summary>
    Task<FileToDownload> GetFileToDownloadAsync(Guid fileId, DownloadFileType fileType);
}