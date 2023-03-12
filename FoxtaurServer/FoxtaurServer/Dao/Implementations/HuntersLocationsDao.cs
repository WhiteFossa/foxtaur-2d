using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Dao.Models;
using Microsoft.EntityFrameworkCore;

namespace FoxtaurServer.Dao.Implementations;

public class HuntersLocationsDao : IHuntersLocationsDao
{
    private readonly MainDbContext _dbContext;

    public HuntersLocationsDao(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IReadOnlyCollection<HunterLocation>> GetHuntersLocationsByHuntersIdsAsync(IReadOnlyCollection<Guid> huntersIds, DateTime fromTime)
    {
        _ = huntersIds ?? throw new ArgumentNullException(nameof(huntersIds));

        var stringedHuntersIds = huntersIds
            .Select(hid => hid.ToString())
            .ToList();

        return _dbContext
            .HuntersLocations
            .Include(hl => hl.Hunter)
            .Where(hl => hl.Timestamp >= fromTime)
            .Where(hl => stringedHuntersIds.Contains(hl.Hunter.Id))
            .ToList();
    }

    public async Task MassCreateAsync(IReadOnlyCollection<HunterLocation> huntersLocations)
    {
        _ = huntersLocations ?? throw new ArgumentNullException(nameof(huntersLocations));

        foreach (var location in huntersLocations)
        {
            await LoadLinkedEntitiesAsync(location);
        
            await _dbContext.HuntersLocations.AddAsync(location);
        }
        
        var affected = await _dbContext.SaveChangesAsync();
        if (affected != huntersLocations.Count)
        {
            throw new InvalidOperationException($"Expected to insert { huntersLocations.Count } row, actually inserted { affected } rows!");
        }
    }

    public async Task MassUpdateAsync(IReadOnlyCollection<HunterLocation> huntersLocations)
    {
        _ = huntersLocations ?? throw new ArgumentNullException(nameof(huntersLocations));

        var huntersLocationsIds = huntersLocations
            .Select(hl => hl.Id)
            .ToList();

        var existingLocations = _dbContext
            .HuntersLocations
            .Include(hl => hl.Hunter)
            .Where(hl => huntersLocationsIds.Contains(hl.Id))
            .ToList();

        var huntersIds = huntersLocations
            .Select(hl => hl.Hunter.Id)
            .ToList();
        
        var hunters = _dbContext
            .Profiles
            .Include(p => p.Team)
            .Where(p => huntersIds.Contains(p.Id))
            .ToList();

        foreach (var hunterLocation in huntersLocations)
        {
            var existingLocation = existingLocations.Single(el => el.Id == hunterLocation.Id);

            existingLocation.Timestamp = hunterLocation.Timestamp;
            existingLocation.Lat = hunterLocation.Lat;
            existingLocation.Lon = hunterLocation.Lon;
            existingLocation.Alt = hunterLocation.Alt;
            existingLocation.Hunter = hunters.Single(h => h.Id == hunterLocation.Hunter.Id);
        }
        
        await _dbContext.SaveChangesAsync();
    }

    private async Task LoadLinkedEntitiesAsync(HunterLocation hunterLocation)
    {
        _ = hunterLocation ?? throw new ArgumentNullException(nameof(hunterLocation));

        hunterLocation.Hunter = await LoadLinkedHunterAsync(hunterLocation.Hunter);
    }
    
    private async Task<Profile> LoadLinkedHunterAsync(Profile hunter)
    {
        if (hunter == null)
        {
            return null;
        }

        var loadedHunter = await _dbContext.Profiles.SingleOrDefaultAsync(p => p.Id.ToLower() == hunter.Id.ToLower());
        if (loadedHunter == null)
        {
            throw new ArgumentException(nameof(hunter));
        }

        return loadedHunter;
    }
}