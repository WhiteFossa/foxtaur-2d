using FoxtaurServer.Dao.Models;
using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Mappers.Abstract;

/// <summary>
/// GSM-interfaced GPS trackers mapper
/// </summary>
public interface IGsmGpsTrackersMapper
{
    IReadOnlyCollection<GsmGpsTrackerDto> Map(IEnumerable<GsmGpsTracker> trackers);

    GsmGpsTrackerDto Map(GsmGpsTracker tracker);

    GsmGpsTracker Map(GsmGpsTrackerDto tracker);

    IReadOnlyCollection<GsmGpsTracker> Map(IEnumerable<GsmGpsTrackerDto> trackers);
}