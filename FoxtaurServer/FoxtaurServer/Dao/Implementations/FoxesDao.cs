using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Dao.Models;

namespace FoxtaurServer.Dao.Implementations;

public class FoxesDao : IFoxesDao
{
    private readonly MainDbContext _dbContext;

    public FoxesDao(MainDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Fox>> GetFoxesAsync(IReadOnlyCollection<Guid> foxesIds)
    {
        _ = foxesIds ?? throw new ArgumentNullException(nameof(foxesIds));
        
        return _dbContext
            .Foxes
            .Where(f => foxesIds.Contains(f.Id))
            .ToList();
    }

    public async Task<Fox> GetFoxByNameAsync(string name)
    {
        return _dbContext
            .Foxes
            .SingleOrDefault(f => f.Name.ToLower().Equals(name.ToLower()));
    }

    public async Task<IReadOnlyCollection<Fox>> GetAllFoxesAsync()
    {
        return _dbContext
            .Foxes
            .ToList();
    }

    public async Task CreateAsync(Fox fox)
    {
        _ = fox ?? throw new ArgumentNullException(nameof(fox));

        await _dbContext.Foxes.AddAsync(fox);
        
        var affected = await _dbContext.SaveChangesAsync();
        if (affected != 1)
        {
            throw new InvalidOperationException($"Expected to insert 1 row, actually inserted { affected } rows!");
        }
    }
}