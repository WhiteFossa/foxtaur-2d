using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Services.Abstract;

/// <summary>
/// Service to work with map files
/// </summary>
public interface IMapFilesService
{
    /// <summary>
    /// Get all existing map files
    /// </summary>
    Task<IReadOnlyCollection<MapFileDto>> GetAllMapFilesAsync();

    /// <summary>
    /// Create new map file.
    /// </summary>
    /// <param name="mapFile">Map file. ID is IGNORED.</param>
    /// <param name="size">Expected size of map file.</param>
    Task<MapFileDto> CreateNewMapFileAsync(MapFileDto mapFile, int size);

    /// <summary>
    /// Upload part of map file
    /// </summary>
    /// <param name="id">Upload into this map file.</param>
    /// <param name="startPosition">Upload data starting from this position.</param>
    /// <param name="data">Data to upload.</param>
    Task UploadMapFilePartAsync(Guid id, int startPosition, byte[] data);

    /// <summary>
    /// Mark map file as ready. Call this after last chunk upload.
    /// </summary>
    Task MarkMapFileAsReadyAsync(Guid id);

    /// <summary>
    /// Get map file storage path by map file ID
    /// </summary>
    Task<string> GetMapFilePathByIdAsync(Guid id);
}