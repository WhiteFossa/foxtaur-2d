using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Dao.Models;
using Microsoft.EntityFrameworkCore;

namespace FoxtaurServer.Dao.Implementations;

public class DistanceToFoxLocationLinkersDao : IDistanceToFoxLocationLinkersDao
{
    private readonly MainDbContext _dbContext;

    public DistanceToFoxLocationLinkersDao(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task MassCreateAsync(IReadOnlyCollection<DistanceToFoxLocationLinker> linkers)
    {
        _ = linkers ?? throw new ArgumentNullException(nameof(linkers));

        foreach (var linker in linkers)
        {
            await LoadLinkedEntitiesAsync(linker);
        
            await _dbContext.DistanceToFoxLocationLinkers.AddAsync(linker);
        }
        
        var affected = await _dbContext.SaveChangesAsync();
        if (affected != linkers.Count)
        {
            throw new InvalidOperationException($"Expected to insert { linkers.Count } row, actually inserted { affected } rows!");
        }
    }
    
    private async Task LoadLinkedEntitiesAsync(DistanceToFoxLocationLinker linker)
    {
        _ = linker ?? throw new ArgumentNullException(nameof(linker));

        linker.Distance = await LoadLinkedDistanceAsync(linker.Distance);
        linker.FoxLocation = await LoadLinkedFoxLocationAsync(linker.FoxLocation);
    }
    
    private async Task<Distance> LoadLinkedDistanceAsync(Distance distance)
    {
        if (distance == null)
        {
            return null;
        }

        var loadedDistance = await _dbContext.Distances.SingleOrDefaultAsync(d => d.Id == distance.Id);
        if (loadedDistance == null)
        {
            throw new ArgumentException(nameof(distance));
        }

        return loadedDistance;
    }
    
    private async Task<Location> LoadLinkedFoxLocationAsync(Location location)
    {
        if (location == null)
        {
            return null;
        }

        var loadedLocation = await _dbContext.Locations.SingleOrDefaultAsync(l => l.Id == location.Id);
        if (loadedLocation == null)
        {
            throw new ArgumentException(nameof(location));
        }

        return loadedLocation;
    }
}