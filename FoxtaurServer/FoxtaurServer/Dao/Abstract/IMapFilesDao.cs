using FoxtaurServer.Dao.Models;

namespace FoxtaurServer.Dao.Abstract;

/// <summary>
/// Interface to work with map files
/// </summary>
public interface IMapFilesDao
{
    #region Gets

    /// <summary>
    /// Get all map files.
    /// </summary>
    Task<IReadOnlyCollection<MapFile>> GetAllAsync();

    /// <summary>
    /// Get single map file by ID
    /// </summary>
    Task<MapFile> GetByIdAsync(Guid id);
    
    #endregion
    
    #region Create and update
    
    /// <summary>
    /// Create new map file. ID will be written into mapFile.Id
    /// </summary>
    Task CreateAsync(MapFile mapFile);

    /// <summary>
    /// Call this to mark map file as ready for use
    /// </summary>
    Task MarkAsReadyAsync(Guid id);

    /// <summary>
    /// Updates map file
    /// </summary>
    Task UpdateAsync(MapFile newData);

    #endregion
}