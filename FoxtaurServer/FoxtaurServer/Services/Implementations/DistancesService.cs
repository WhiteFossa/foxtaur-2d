using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Dao.Models;
using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;

namespace FoxtaurServer.Services.Implementations;

public class DistancesService : IDistancesService
{
    private readonly IDistancesDao _distancesDao;
    private readonly IDistancesMapper _distancesMapper;
    private readonly IDistanceToFoxLocationLinkersDao _distanceToFoxLocationLinkersDao;

    public DistancesService(IDistancesDao distancesDao,
        IDistancesMapper distancesMapper,
        IDistanceToFoxLocationLinkersDao distanceToFoxLocationLinkersDao)
    {
        _distancesDao = distancesDao;
        _distancesMapper = distancesMapper;
        _distanceToFoxLocationLinkersDao = distanceToFoxLocationLinkersDao;
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

        // Creating fox orders locations linkers
        var foxesLocationsIds = distance.FoxesLocationsIds.ToList();
        
        var linkers = new List<DistanceToFoxLocationLinker>();
        for (var fli = 0; fli < foxesLocationsIds.Count; fli ++)
        {
            linkers.Add(new DistanceToFoxLocationLinker()
            {
                Distance = mappedDistance,
                FoxLocation = new Location() { Id = foxesLocationsIds[fli] }, // DAO will load full location
                Order = fli
            });
        }

        await _distanceToFoxLocationLinkersDao.MassCreateAsync(linkers);

        mappedDistance.FoxesLocations = linkers;
        await _distancesDao.UpdateAsync(mappedDistance);
        
        return new DistanceDto(
            mappedDistance.Id,
            distance.Name,
            distance.MapId,
            distance.IsActive,
            distance.StartLocationId,
            distance.FinishCorridorEntranceLocationId,
            distance.FinishLocationId,
            distance.FoxesLocationsIds,
            distance.HuntersIds,
            distance.FirstHunterStartTime);
    }
}