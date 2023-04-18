using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;

namespace FoxtaurServer.Services.Implementations;

public class GsmGpsTrackersService : IGsmGpsTrackersService
{
    private readonly IGsmGpsTrackersDao _trackersDao;
    private readonly IGsmGpsTrackersMapper _trackersMapper;

    public GsmGpsTrackersService
    (
        IGsmGpsTrackersDao trackersDao,
        IGsmGpsTrackersMapper trackersMapper
    )
    {
        _trackersDao = trackersDao;
        _trackersMapper = trackersMapper;
    }
    
    public async Task<IReadOnlyCollection<GsmGpsTrackerDto>> GetAllTrackersAsync()
    {
        return _trackersMapper.Map(await _trackersDao.GetAllTrackersAsync().ConfigureAwait(false));
    }

    public async Task<GsmGpsTrackerDto> CreateNewTrackerAsync(GsmGpsTrackerDto tracker)
    {
        _ = tracker ?? throw new ArgumentNullException(nameof(tracker), "Tracker must be specified.");

        var mappedTracker = _trackersMapper.Map(tracker);
        
        await _trackersDao.CreateAsync(mappedTracker);

        return new GsmGpsTrackerDto(mappedTracker.Id, tracker.Imei, tracker.UsedBy);
    }
}