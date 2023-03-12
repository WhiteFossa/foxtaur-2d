using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Dao.Models;
using Microsoft.EntityFrameworkCore;

namespace FoxtaurServer.Dao.Implementations;

public class DistancesDao : IDistancesDao
{
    private readonly MainDbContext _dbContext;

    public DistancesDao(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IReadOnlyCollection<Distance>> GetAllDistancesAsync()
    {
        return PrepareGetDistancesConstantQueryPart()
            .ToList();
    }

    public Task<Distance> GetDistanceByIdAsync(Guid distanceId)
    {
        return PrepareGetDistancesConstantQueryPart()
            .SingleOrDefaultAsync(d => d.Id == distanceId);
    }

    public Task<Distance> GetDistanceByNameAsync(string name)
    {
        return PrepareGetDistancesConstantQueryPart()
            .SingleOrDefaultAsync(d => d.Name.ToLower().Equals(name.ToLower()));
    }

    public async Task CreateAsync(Distance distance)
    {
        _ = distance ?? throw new ArgumentNullException(nameof(distance));

        await LoadLinkedEntitiesAsync(distance);
        
        await _dbContext.Distances.AddAsync(distance);
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Distance distance)
    {
        _ = distance ?? throw new ArgumentNullException(nameof(distance));
        
        var oldDistance = await GetDistanceByIdAsync(distance.Id);
        if (oldDistance == null)
        {
            throw new ArgumentException(nameof(distance.Id));
        }

        await LoadLinkedEntitiesAsync(distance);

        oldDistance.Name = distance.Name;
        oldDistance.Map = distance.Map;
        oldDistance.IsActive = distance.IsActive;
        oldDistance.StartLocation = distance.StartLocation;
        oldDistance.FinishCorridorEntranceLocation = distance.FinishCorridorEntranceLocation;
        oldDistance.FinishLocation = distance.FinishLocation;
        oldDistance.FoxesLocations = distance.FoxesLocations;
        oldDistance.Hunters = distance.Hunters;
        oldDistance.FirstHunterStartTime = distance.FirstHunterStartTime;
        
        await _dbContext.SaveChangesAsync();
    }

    private async Task LoadLinkedEntitiesAsync(Distance distance)
    {
        _ = distance ?? throw new ArgumentNullException(nameof(distance));

        distance.StartLocation = await LoadLinkedLocationAsync(distance.StartLocation);
        distance.FinishCorridorEntranceLocation = await LoadLinkedLocationAsync(distance.FinishCorridorEntranceLocation);
        distance.FinishLocation = await LoadLinkedLocationAsync(distance.FinishLocation);

        distance.Map = await LoadLinkedMapAsync(distance.Map);

        for (var fli = 0; fli < distance.FoxesLocations.Count; fli ++)
        {
            distance.FoxesLocations[fli] = await LoadLinkedDistanceToFoxLocationLinkerAsync(distance.FoxesLocations[fli]);
        }

        for (var hi = 0; hi < distance.Hunters.Count; hi++)
        {
            distance.Hunters[hi] = await LoadLinkedHunterAsync(distance.Hunters[hi]);
        }
    }

    private async Task<Location> LoadLinkedLocationAsync(Location location)
    {
        if (location == null)
        {
            return null;
        }

        var loadedLocation = await _dbContext
            .Locations
            .Include(l => l.Fox)
            .SingleOrDefaultAsync(l => l.Id == location.Id);
        if (loadedLocation == null)
        {
            throw new ArgumentException(nameof(location));
        }

        return loadedLocation;
    }
    
    private async Task<DistanceToFoxLocationLinker> LoadLinkedDistanceToFoxLocationLinkerAsync(DistanceToFoxLocationLinker linker)
    {
        if (linker == null)
        {
            return null;
        }

        var loadedLinker = await _dbContext
            .DistanceToFoxLocationLinkers
            .Include(l => l.FoxLocation)
            .ThenInclude(fl => fl.Fox)
            .SingleOrDefaultAsync(l => l.Id == linker.Id);
        if (loadedLinker == null)
        {
            throw new ArgumentException(nameof(linker));
        }

        return loadedLinker;
    }
    
    private async Task<Profile> LoadLinkedHunterAsync(Profile profile)
    {
        if (profile == null)
        {
            return null;
        }

        var loadedProfile = await _dbContext
            .Profiles
            .Include(p => p.Team)
            .SingleOrDefaultAsync(p => p.Id == profile.Id);
        if (loadedProfile == null)
        {
            throw new ArgumentException(nameof(profile));
        }

        return loadedProfile;
    }
    
    private async Task<Map> LoadLinkedMapAsync(Map map)
    {
        if (map == null)
        {
            return null;
        }

        var loadedMap = await _dbContext
            .Maps
            .SingleOrDefaultAsync(m => m.Id == map.Id);
        if (loadedMap == null)
        {
            throw new ArgumentException(nameof(map));
        }

        return loadedMap;
    }
    
    private IQueryable<Distance> PrepareGetDistancesConstantQueryPart()
    {
        return _dbContext
            .Distances
            .Include(d => d.Map)
            .Include(d => d.StartLocation) // We know that on usual locations there is no foxes
            .Include(d => d.FinishCorridorEntranceLocation)
            .Include(d => d.FinishLocation)
            .Include(d => d.FoxesLocations)
            .ThenInclude(l => l.FoxLocation)
            .ThenInclude(fl => fl.Fox)
            .Include(d => d.Hunters)
            .ThenInclude(h => h.Team);
    }
}