using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Dao.Models;
using Microsoft.EntityFrameworkCore;

namespace FoxtaurServer.Dao.Implementations;

public class LocationsDao : ILocationsDao
{
    private readonly MainDbContext _dbContext;

    public LocationsDao(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IReadOnlyCollection<Location>> GetLocationsAsync(IReadOnlyCollection<Guid> locationsIds)
    {
        _ = locationsIds ?? throw new ArgumentNullException(nameof(locationsIds));
        
        return _dbContext
            .Locations
            .Include(l => l.Fox)
            .Where(f => locationsIds.Contains(f.Id))
            .ToList();
    }

    public async Task<IReadOnlyCollection<Location>> GetAllLocationsAsync()
    {
        return _dbContext
            .Locations
            .Include(l => l.Fox)
            .ToList();
    }

    public async Task CreateAsync(Location location)
    {
        _ = location ?? throw new ArgumentNullException(nameof(location));

        await LoadLinkedEntitiesAsync(location);
        
        await _dbContext.Locations.AddAsync(location);
        
        var affected = await _dbContext.SaveChangesAsync();
        if (affected != 1)
        {
            throw new InvalidOperationException($"Expected to insert 1 row, actually inserted { affected } rows!");
        }
    }
    
    private async Task LoadLinkedEntitiesAsync(Location location)
    {
        _ = location ?? throw new ArgumentNullException(nameof(location));

        location.Fox = await LoadLinkedFoxAsync(location.Fox);
    }
    
    private async Task<Fox> LoadLinkedFoxAsync(Fox fox)
    {
        if (fox == null)
        {
            return null;
        }

        var loadedFox = await _dbContext.Foxes.SingleOrDefaultAsync(f => f.Id == fox.Id);
        if (loadedFox == null)
        {
            throw new ArgumentException(nameof(fox));
        }

        return loadedFox;
    }
}