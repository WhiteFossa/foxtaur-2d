using Binateq.GpsTrackFilter;
using LibGeo.Implementations.Helpers;
using LibGpsFilter.Abstractions;
using LibWebClient.Models;

namespace LibGpsFilter.Implementations;

public class GpsFilter : IGpsFilter
{
    private readonly GpsTrackFilter _filter;

    private Mutex _filteringMutex = new Mutex();
    
    public GpsFilter()
    {
        _filter = new GpsTrackFilter();
        _filter.ZeroSpeedDrift = 1.0; // Minimal hunter speed
        _filter.OutlineSpeed = 20.0; // Maximal hunter speed
    }
    
    public IReadOnlyCollection<GpsLocation> FilterLocations(IReadOnlyCollection<GpsLocation> locations)
    {
        try
        {
            _filteringMutex.WaitOne();
            
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
                .Select(ftl => new GpsLocation(ftl.Item3, ftl.Item1.ToRadians(), ftl.Item2.ToRadians()))
                .ToList();
        }
        finally
        {
            _filteringMutex.ReleaseMutex();
        }
    }
}