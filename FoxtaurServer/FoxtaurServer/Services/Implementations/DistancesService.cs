using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;

namespace FoxtaurServer.Services.Implementations;

public class DistancesService : IDistancesService
{
    private readonly IDistancesDao _distancesDao;
    private readonly IDistancesMapper _distancesMapper;

    public DistancesService(IDistancesDao distancesDao,
        IDistancesMapper distancesMapper)
    {
        _distancesDao = distancesDao;
        _distancesMapper = distancesMapper;
    }
    
    public async Task<IReadOnlyCollection<DistanceDto>> ListDistancesAsync()
    {
        return _distancesMapper.Map(await _distancesDao.GetAllDistancesAsync());
    }

    public async Task<DistanceDto> GetDistanceById(Guid id)
    {
        return _distancesMapper.Map(await _distancesDao.GetDistanceByIdAsync(id));
    }

    public async Task<DistanceDto> CreateNewDistanceAsync(DistanceDto distance)
    {
        _ = distance ?? throw new ArgumentNullException(nameof(distance));
        
        // Do we have distance with such name?
        var existingDistance = await _distancesDao.GetDistanceByNameAsync(distance.Name);
        if (existingDistance != null)
        {
            return null;
        }
        
        var mappedDistance = _distancesMapper.Map(distance);

        await _distancesDao.CreateAsync(mappedDistance);

        return new DistanceDto(
            mappedDistance.Id,
            distance.Name,
            distance.MapId,
            distance.IsActive,
            distance.StartLocationId,
            distance.FinishCorridorEntranceLocationId,
            distance.FinishLocationId,
            distance.FoxesLocationsIds,
            distance.ExpectedFoxesOrderLocationsIds,
            distance.HuntersIds,
            distance.FirstHunterStartTime);
    }
}