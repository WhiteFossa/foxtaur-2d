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

        await _dbContext.MapFiles.AddAsync(mapFile);
        
        var affected = await _dbContext.SaveChangesAsync();
        if (affected != 1)
        {
            throw new InvalidOperationException($"Expected to insert 1 row, actually inserted { affected } rows!");
        }
    }
}