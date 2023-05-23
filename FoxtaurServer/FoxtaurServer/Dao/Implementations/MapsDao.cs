using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Dao.Models;
using Microsoft.EntityFrameworkCore;

namespace FoxtaurServer.Dao.Implementations;

public class MapsDao : IMapsDao
{
    private readonly MainDbContext _dbContext;

    public MapsDao(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Map>> GetMapsAsync(IReadOnlyCollection<Guid> mapsIds)
    {
        _ = mapsIds ?? throw new ArgumentNullException(nameof(mapsIds));
        
        return _dbContext
            .Maps
            .Include(m => m.File)
            .Where(t => mapsIds.Contains(t.Id))
            .ToList();
    }

    public async Task<Map> GetMapByNameAsync(string name)
    {
        return _dbContext
            .Maps
            .Include(m => m.File)
            .SingleOrDefault(m => m.Name.ToLower().Equals(name.ToLower()));
    }

    public async Task<IReadOnlyCollection<Map>> GetAllMapsAsync()
    {
        return _dbContext
            .Maps
            .Include(m => m.File)
            .ToList();
    }

    public async Task CreateAsync(Map map)
    {
        _ = map ?? throw new ArgumentNullException(nameof(map));

        var mapFile = _dbContext
            .MapFiles
            .SingleOrDefault(mf => mf.Id == map.File.Id);

        if (mapFile == null)
        {
            throw new ArgumentException("Map file with given ID is not found.", nameof(map.File.Id));
        }

        map.File = mapFile;
        
        await _dbContext.Maps.AddAsync(map);
        
        var affected = await _dbContext.SaveChangesAsync();
        if (affected != 1)
        {
            throw new InvalidOperationException($"Expected to insert 1 row, actually inserted { affected } rows!");
        }
    }
}