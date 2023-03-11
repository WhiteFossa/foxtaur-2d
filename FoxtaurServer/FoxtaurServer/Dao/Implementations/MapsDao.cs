using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Dao.Models;

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
            .Where(t => mapsIds.Contains(t.Id))
            .ToList();
    }

    public async Task<Map> GetMapByNameAsync(string name)
    {
        return _dbContext
            .Maps
            .SingleOrDefault(m => m.Name.ToLower().Equals(name.ToLower()));
    }

    public async Task<IReadOnlyCollection<Map>> GetAllMapsAsync()
    {
        return _dbContext
            .Maps
            .ToList();
    }

    public async Task CreateAsync(Map map)
    {
        _ = map ?? throw new ArgumentNullException(nameof(map));

        await _dbContext.Maps.AddAsync(map);
        
        var affected = await _dbContext.SaveChangesAsync();
        if (affected != 1)
        {
            throw new InvalidOperationException($"Expected to insert 1 row, actually inserted { affected } rows!");
        }
    }
}