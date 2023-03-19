using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Dao.Models;
using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;

namespace FoxtaurServer.Services.Implementations;

public class HuntersLocationsService : IHuntersLocationsService
{
    private readonly IHuntersLocationsDao _huntersLocationsDao;
    private readonly IHuntersLocationsMapper _huntersLocationsMapper;

    public HuntersLocationsService(IHuntersLocationsDao huntersLocationsDao,
        IHuntersLocationsMapper huntersLocationsMapper)
    {
        _huntersLocationsDao = huntersLocationsDao;
        _huntersLocationsMapper = huntersLocationsMapper;
    }
    
    public async Task<Dictionary<Guid, IReadOnlyCollection<HunterLocationDto>>> MassGetHuntersLocationsAsync(IReadOnlyCollection<Guid> huntersIds, DateTime fromTime)
    {
        _ = huntersIds ?? throw new ArgumentNullException(nameof(huntersIds));
        
        return (await _huntersLocationsDao.GetHuntersLocationsByHuntersIdsAsync(huntersIds, fromTime))
            .GroupBy(hl => hl.Hunter.Id)
            .ToDictionary(g => Guid.Parse(g.Key), g => _huntersLocationsMapper.Map(g.ToList()));
    }

    public async Task<IReadOnlyCollection<Guid>> MassCreateHuntersLocationsAsync(IReadOnlyCollection<HunterLocationDto> huntersLocations, Guid hunterId)
    {
        _ = huntersLocations ?? throw new ArgumentNullException(nameof(huntersLocations));

        var incomingHunterLocationsIds = huntersLocations
            .Select(hl => hl.Id)
            .ToList();
        
        var existingHunterLocationsIds = (await _huntersLocationsDao.GetHuntersLocationsByHuntersIdsAsync(new List<Guid>() { hunterId }, DateTime.MinValue))
            .Select(hl => hl.Id)
            .ToList();

        var hunterLocationsForDb = _huntersLocationsMapper.Map(huntersLocations);
        foreach (var hunterLocationForDb in hunterLocationsForDb)
        {
            hunterLocationForDb.Hunter = new Profile() { Id = hunterId.ToString() }; // DAO must restore full profile by ID 
        }

        var toInsert = hunterLocationsForDb
            .Where(hltdb => !existingHunterLocationsIds.Contains(hltdb.Id))
            .ToList();
        
        var toUpdate = hunterLocationsForDb
            .Where(hltdb => existingHunterLocationsIds.Contains(hltdb.Id))
            .ToList();
        
        await _huntersLocationsDao.MassCreateAsync(toInsert);
        await _huntersLocationsDao.MassUpdateAsync(toUpdate);

        // TODO: Maybe it will be good to check if locations are actually saved?
        return incomingHunterLocationsIds;
    }
}