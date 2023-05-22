using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Dao.Models;

namespace FoxtaurServer.Dao.Implementations;

public class MapFilesDao : IMapFilesDao
{
    private readonly MainDbContext _dbContext;

    public MapFilesDao(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<MapFile>> GetAllAsync()
    {
        return _dbContext
            .MapFiles
            .ToList();
    }

    public async Task<MapFile> GetByIdAsync(Guid id)
    {
        return _dbContext
            .MapFiles
            .SingleOrDefault(mf => mf.Id == id);
    }

    public async Task CreateAsync(MapFile mapFile)
    {
        _ = mapFile ?? throw new ArgumentNullException(nameof(mapFile));

        mapFile.CreationTime = DateTime.UtcNow;
        
        await _dbContext.MapFiles.AddAsync(mapFile);
        
        var affected = await _dbContext.SaveChangesAsync();
        if (affected != 1)
        {
            throw new InvalidOperationException($"Expected to insert 1 row, actually inserted { affected } rows!");
        }
    }

    public async Task MarkAsReadyAsync(Guid id)
    {
        var mapFile = await GetByIdAsync(id);
        if (mapFile.IsReady)
        {
            throw new InvalidOperationException($"Map file with ID={id} is already marked as ready.");
        }

        mapFile.IsReady = true;
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(MapFile newData)
    {
        _ = newData ?? throw new ArgumentNullException(nameof(newData));

        var mapFile = await GetByIdAsync(newData.Id);
        if (mapFile == null)
        {
            throw new ArgumentException($"Map file with Id = {newData.Id} not found!", nameof(newData.Id));
        }

        mapFile.Name = newData.Name;
        mapFile.Hash = newData.Hash;

        var affected = await _dbContext.SaveChangesAsync();
        if (affected != 1)
        {
            throw new InvalidOperationException($"Expected to update 1 row, actually updated { affected } rows!");
        }
    }
}