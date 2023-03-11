using FoxtaurServer.Dao.Models;

namespace FoxtaurServer.Dao.Abstract;

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

    public async Task<IReadOnlyCollection<Map>> GetAllMapsAsync()
    {
        return _dbContext
            .Maps
            .ToList();
    }
}