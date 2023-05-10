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
}