using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;

namespace FoxtaurServer.Services.Implementations;

public class FoxesService : IFoxesService
{
    private readonly IFoxesDao _foxesDao;
    private readonly IFoxesMapper _foxesMapper;

    public FoxesService(IFoxesDao foxesDao,
        IFoxesMapper foxesMapper)
    {
        _foxesDao = foxesDao;
        _foxesMapper = foxesMapper;
    }

    public async Task<IReadOnlyCollection<FoxDto>> MassGetFoxesAsync(IReadOnlyCollection<Guid> foxesIds)
    {
        _ = foxesIds ?? throw new ArgumentNullException(nameof(foxesIds));

        return _foxesMapper.Map(await _foxesDao.GetFoxesAsync(foxesIds));
    }

    public async Task<IReadOnlyCollection<FoxDto>> GetAllFoxesAsync()
    {
        return _foxesMapper.Map(await _foxesDao.GetAllFoxesAsync());
    }

    public async Task<FoxDto> CreateNewFoxAsync(FoxDto fox)
    {
        _ = fox ?? throw new ArgumentNullException(nameof(fox));
                
        // Do we have fox with such name?
        var existingFox = await _foxesDao.GetFoxByNameAsync(fox.Name);
        if (existingFox != null)
        {
            return null;
        }
        
        var mappedFox = _foxesMapper.Map(fox);
        
        await _foxesDao.CreateAsync(mappedFox);

        return new FoxDto(mappedFox.Id, fox.Name, fox.Frequency, fox.Code);
    }
}