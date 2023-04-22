using FoxtaurServer.Dao.Models;
using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Mappers.Implementations;

public class GsmGpsTrackersMapper : IGsmGpsTrackersMapper
{
    public IReadOnlyCollection<GsmGpsTrackerDto> Map(IEnumerable<GsmGpsTracker> trackers)
    {
        if (trackers == null)
        {
            return null;
        }

        return trackers.Select(t => Map(t)).ToList();
    }

    public GsmGpsTrackerDto Map(GsmGpsTracker tracker)
    {
        if (tracker == null)
        {
            return null;
        }
        
        return new GsmGpsTrackerDto
        (
            tracker.Id,
            tracker.Imei,
            tracker.Name,
            tracker.UsedBy != null
            ? Guid.Parse(tracker.UsedBy.Id)
            : null
        );
    }

    public GsmGpsTracker Map(GsmGpsTrackerDto tracker)
    {
        if (tracker == null)
        {
            return null;
        }
        
        return new GsmGpsTracker()
        {
            Id = tracker.Id,
            Imei = tracker.Imei,
            Name = tracker.Name,
            UsedBy = tracker.UsedBy.HasValue ? new Profile() { Id = tracker.UsedBy.Value.ToString() } : null // Load other parts of profile outside
        };
    }

    public IReadOnlyCollection<GsmGpsTracker> Map(IEnumerable<GsmGpsTrackerDto> trackers)
    {
        if (trackers == null)
        {
            return null;
        }

        return trackers.Select(t => Map(t)).ToList();
    }
}