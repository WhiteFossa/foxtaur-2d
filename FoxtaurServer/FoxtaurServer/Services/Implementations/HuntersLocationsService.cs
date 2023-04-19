using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Dao.Models;
using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;

namespace FoxtaurServer.Services.Implementations;

public class HuntersLocationsService : IHuntersLocationsService
{
    private readonly ILogger _logger;

    private readonly IHuntersLocationsDao _huntersLocationsDao;
    private readonly IHuntersLocationsMapper _huntersLocationsMapper;
    private readonly IGsmGpsTrackersDao _gsmGpsTrackersDao;

    public HuntersLocationsService(ILogger<HuntersLocationsService> logger,
        IHuntersLocationsDao huntersLocationsDao,
        IHuntersLocationsMapper huntersLocationsMapper,
        IGsmGpsTrackersDao gsmGpsTrackersDao)
    {
        _logger = logger;
        _huntersLocationsDao = huntersLocationsDao;
        _huntersLocationsMapper = huntersLocationsMapper;
        _gsmGpsTrackersDao = gsmGpsTrackersDao;
    }
    
    public async Task<Dictionary<Guid, IReadOnlyCollection<HunterLocationDto>>> MassGetHuntersLocationsAsync(IReadOnlyCollection<Guid> huntersIds, DateTime fromTime, DateTime toTime)
    {
        _ = huntersIds ?? throw new ArgumentNullException(nameof(huntersIds));
        
        return (await _huntersLocationsDao.GetHuntersLocationsByHuntersIdsAsync(huntersIds, fromTime, toTime))
            .GroupBy(hl => hl.Hunter.Id)
            .ToDictionary(g => Guid.Parse(g.Key), g => _huntersLocationsMapper.Map(g.ToList()));
    }

    public async Task<IReadOnlyCollection<Guid>> MassCreateHuntersLocationsAsync(IReadOnlyCollection<HunterLocationDto> huntersLocations, Guid hunterId)
    {
        _ = huntersLocations ?? throw new ArgumentNullException(nameof(huntersLocations));

        var incomingHunterLocationsIds = huntersLocations
            .Select(hl => hl.Id)
            .ToList();
        
        var existingHunterLocationsIds = (await _huntersLocationsDao.GetHuntersLocationsByHuntersIdsAsync(new List<Guid>() { hunterId }, DateTime.MinValue, DateTime.MaxValue))
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

    public async Task CreateHunterLocationFromGsmGpsTracker(string imei, DateTime time, double lat, double lon)
    {
        if (string.IsNullOrWhiteSpace(imei))
        {
            throw new ArgumentException("Empty IMEI is not allowed.", nameof(imei));
        }

        // Do we have a tracker with given IMEI?
        var tracker = await _gsmGpsTrackersDao.GetByImeiAsync(imei).ConfigureAwait(false);
        if (tracker == null)
        {
            _logger.LogWarning($"Foreign tracker with IMEI = { imei }, ignoring.");
            return;
        }
        
        // Hunter, using this tracker
        if (tracker.UsedBy == null)
        {
            _logger.LogWarning($"Tracker with IMEI = { imei } is used by no one, ignoring.");
            return;
        }

        var location = new HunterLocation()
        {
            Hunter = tracker.UsedBy,
            Timestamp = time,
            Lat = lat,
            Lon = lon,
            Alt = 0
        };

        await _huntersLocationsDao.MassCreateAsync(new List<HunterLocation>() { location }).ConfigureAwait(false);
    }
}