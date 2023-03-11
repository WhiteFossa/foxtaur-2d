using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Dao.Models;

namespace FoxtaurServer.Dao.Implementations;

public class ProfilesDao : IProfilesDao
{
    private readonly MainDbContext _dbContext;

    public ProfilesDao(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IReadOnlyCollection<Profile>> GetProfilesAsync(IReadOnlyCollection<string> profilesIds)
    {
        _ = profilesIds ?? throw new ArgumentNullException(nameof(profilesIds));
        
        return _dbContext
            .Profiles
            .Where(p => profilesIds.Contains(p.Id))
            .ToList();
    }

    public async Task CreateAsync(Profile profile)
    {
        _ = profile ?? throw new ArgumentNullException(nameof(profile));
        
        await _dbContext.Profiles.AddAsync(profile);
        
        var affected = await _dbContext.SaveChangesAsync();
        if (affected != 1)
        {
            throw new InvalidOperationException($"Expected to insert 1 row, actually inserted { affected } rows!");
        }
    }
}