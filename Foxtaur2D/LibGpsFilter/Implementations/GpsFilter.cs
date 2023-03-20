using Binateq.GpsTrackFilter;
using LibGeo.Implementations.Helpers;
using LibGpsFilter.Abstractions;
using LibWebClient.Models;

namespace LibGpsFilter.Implementations;

public class GpsFilter : IGpsFilter
{
    private readonly GpsTrackFilter _filter;

    public GpsFilter()
    {
        _filter = new GpsTrackFilter();
        _filter.ZeroSpeedDrift = 0.1; // Minimal hunter speed
        _filter.OutlineSpeed = 30.0; // Maximal hunter speed
    }
    
    public IReadOnlyCollection<GpsLocation> FilterLocations(IReadOnlyCollection<GpsLocation> locations)
    {
        var rawTrack = locations
            .Select(l => new Tuple<double, double, DateTimeOffset>
                (
                    l.Lat.ToDegrees(),
                    l.Lon.ToDegrees(),
                    l.Timestamp
                ))
            .ToList();

        var filteredTrack = _filter.Filter(rawTrack);
        
        return filteredTrack
            .Select(ftl => new GpsLocation(ftl.Item3, ftl.Item1, ftl.Item2))
            .ToList();
    }
}