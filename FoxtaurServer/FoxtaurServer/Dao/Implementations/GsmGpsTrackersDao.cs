using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Dao.Models;
using Microsoft.EntityFrameworkCore;

namespace FoxtaurServer.Dao.Implementations;

public class GsmGpsTrackersDao : IGsmGpsTrackersDao
{
    private readonly MainDbContext _dbContext;

    public GsmGpsTrackersDao(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IReadOnlyCollection<GsmGpsTracker>> GetAllTrackersAsync()
    {
        return _dbContext
            .GsmGpsTrackers
            .Include(t => t.UsedBy)
            .ToList();
    }

    public async Task<GsmGpsTracker> GetByIdAsync(Guid trackerId)
    {
        return await _dbContext
            .GsmGpsTrackers
            .Include(t => t.UsedBy)
            .SingleOrDefaultAsync(t => t.Id == trackerId);
    }

    public async Task<GsmGpsTracker> GetByImeiAsync(string imei)
    {
        if (string.IsNullOrWhiteSpace(imei))
        {
            throw new ArgumentException("IMEI must not be empty.", nameof(imei));
        }
        
        return await _dbContext
            .GsmGpsTrackers
            .Include(t => t.UsedBy)
            .SingleOrDefaultAsync(t => t.Imei.ToLower().Equals(imei.ToLower()));
    }

    public async Task CreateAsync(GsmGpsTracker tracker)
    {
        _ = tracker ?? throw new ArgumentNullException(nameof(tracker), "Tracker must not be null");

        await LoadLinkedEntitiesAsync(tracker);
        
        await _dbContext.GsmGpsTrackers.AddAsync(tracker);
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(GsmGpsTracker tracker)
    {
        _ = tracker ?? throw new ArgumentNullException(nameof(tracker), "Tracker must not be null");

        var oldTracker = await GetByIdAsync(tracker.Id);
        _ = oldTracker ?? throw new ArgumentException("Nonexistent tracker.", nameof(tracker));

        await LoadLinkedEntitiesAsync(oldTracker);

        oldTracker.Imei = tracker.Imei;
        oldTracker.UsedBy = tracker.UsedBy;
        
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid trackerId)
    {
        var tracker = await GetByIdAsync(trackerId);
        _ = tracker ?? throw new ArgumentException("Tracker with given ID not found.", nameof(trackerId));

        _dbContext
            .GsmGpsTrackers
            .Remove(tracker);

        await _dbContext.SaveChangesAsync();
    }

    private async Task LoadLinkedEntitiesAsync(GsmGpsTracker tracker)
    {
        _ = tracker ?? throw new ArgumentNullException(nameof(tracker));

        tracker.UsedBy = await LoadLinkedProfileAsync(tracker.UsedBy);
    }

    private async Task<Profile> LoadLinkedProfileAsync(Profile profile)
    {
        if (profile == null)
        {
            return null;
        }

        var loadedProfile = await _dbContext
            .Profiles
            .SingleOrDefaultAsync(p => p.Id.ToLower().Equals(profile.Id.ToLower()));

        if (loadedProfile == null)
        {
            throw new ArgumentException("Nonexistent profile.", nameof(profile));
        }

        return loadedProfile;
    }
}