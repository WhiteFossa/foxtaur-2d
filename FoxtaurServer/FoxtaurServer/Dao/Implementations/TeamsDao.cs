using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Dao.Models;

namespace FoxtaurServer.Dao.Implementations;

public class TeamsDao : ITeamsDao
{
    private readonly MainDbContext _dbContext;

    public TeamsDao(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<IReadOnlyCollection<Team>> GetTeamsAsync(IReadOnlyCollection<Guid> teamsIds)
    {
        _ = teamsIds ?? throw new ArgumentNullException(nameof(teamsIds));
        
        return _dbContext
            .Teams
            .Where(t => teamsIds.Contains(t.Id))
            .ToList();
    }

    public async Task<Team> GetTeamByNameAsync(string name)
    {
        return _dbContext
            .Teams
            .SingleOrDefault(t => t.Name.ToLower().Equals(name.ToLower()));
    }

    public async Task<IReadOnlyCollection<Team>> GetAllTeamsAsync()
    {
        return _dbContext
            .Teams
            .ToList();
    }

    public async Task CreateAsync(Team team)
    {
        _ = team ?? throw new ArgumentNullException(nameof(team));
        
        await _dbContext.Teams.AddAsync(team);
        
        var affected = await _dbContext.SaveChangesAsync();
        if (affected != 1)
        {
            throw new InvalidOperationException($"Expected to insert 1 row, actually inserted { affected } rows!");
        }
    }
}