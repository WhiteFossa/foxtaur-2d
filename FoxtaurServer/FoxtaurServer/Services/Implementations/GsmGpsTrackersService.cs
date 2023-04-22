using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;

namespace FoxtaurServer.Services.Implementations;

public class GsmGpsTrackersService : IGsmGpsTrackersService
{
    private readonly IGsmGpsTrackersDao _trackersDao;
    private readonly IGsmGpsTrackersMapper _trackersMapper;
    private readonly IProfilesDao _profilesDao;

    public GsmGpsTrackersService
    (
        IGsmGpsTrackersDao trackersDao,
        IGsmGpsTrackersMapper trackersMapper,
        IProfilesDao profilesDao
    )
    {
        _trackersDao = trackersDao;
        _trackersMapper = trackersMapper;
        _profilesDao = profilesDao;
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

        return new GsmGpsTrackerDto(mappedTracker.Id, tracker.Imei, tracker.Name, tracker.UsedBy);
    }

    public async Task<GsmGpsTrackerDto> ClaimTrackerAsync(Guid userId, Guid trackerId)
    {
        var user = (await _profilesDao
                .GetProfilesAsync(new List<string>() { userId.ToString() })
                .ConfigureAwait(false))
                .SingleOrDefault();

        if (user == null)
        {
            // User not found
            return null;
        }

        var tracker = await _trackersDao.GetByIdAsync(trackerId).ConfigureAwait(false);
        if (tracker == null)
        {
            // Tracker not found
            return null;
        }

        tracker.UsedBy = user;
        await _trackersDao.UpdateAsync(tracker).ConfigureAwait(false);

        return _trackersMapper.Map(tracker);
    }
}