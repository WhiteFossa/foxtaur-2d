using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Dao.Models;
using Microsoft.EntityFrameworkCore;

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
            .Include(p => p.Team)
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

    public async Task UpdateAsync(Profile profile)
    {
        _ = profile ?? throw new ArgumentNullException(nameof(profile));

        var oldProfile = (await GetProfilesAsync(new List<string>() { profile.Id })).SingleOrDefault();
        if (oldProfile == null)
        {
            throw new ArgumentException(nameof(profile.Id));
        }

        await LoadLinkedEntitiesAsync(profile);
        
        oldProfile.Id = profile.Id;
        oldProfile.FirstName = profile.FirstName;
        oldProfile.MiddleName = profile.MiddleName;
        oldProfile.LastName = profile.LastName;
        oldProfile.Sex = profile.Sex;
        oldProfile.DateOfBirth = profile.DateOfBirth;
        oldProfile.Phone = profile.Phone;
        oldProfile.Team = profile.Team;
        oldProfile.Category = profile.Category;
        oldProfile.ColorR = profile.ColorR;
        oldProfile.ColorG = profile.ColorG;
        oldProfile.ColorB = profile.ColorB;
        oldProfile.ColorA = profile.ColorA;
        
        await _dbContext.SaveChangesAsync();
    }

    private async Task LoadLinkedEntitiesAsync(Profile profile)
    {
        _ = profile ?? throw new ArgumentNullException(nameof(profile));

        profile.Team = await LoadLinkedTeamAsync(profile.Team);
    }
    
    private async Task<Team> LoadLinkedTeamAsync(Team team)
    {
        if (team == null)
        {
            return null;
        }

        var loadedTeam = await _dbContext.Teams.SingleOrDefaultAsync(t => t.Id == team.Id);
        if (loadedTeam == null)
        {
            throw new ArgumentException(nameof(team));
        }

        return loadedTeam;
    }
}