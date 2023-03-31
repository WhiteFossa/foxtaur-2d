using Binateq.GpsTrackFilter;
using LibGeo.Implementations.Helpers;
using LibGpsFilter.Abstractions;
using LibGpsFilter.Constants;

namespace LibGpsFilter.Implementations;

public class GpsFilter : IGpsFilter
{
    private readonly GpsTrackFilter _filter;

    private Mutex _filteringMutex = new Mutex();
    
    public GpsFilter()
    {
        _filter = new GpsTrackFilter();
        
        _filter.ZeroSpeedDrift = GpsFilterConstants.MinHunterSpeed; // Minimal hunter speed
        _filter.OutlineSpeed = GpsFilterConstants.MaxHunterSpeed; // Maximal hunter speed
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

            List<Tuple<double, double, DateTimeOffset>> filteredTrack = new List<Tuple<double, double, DateTimeOffset>>();

            // Batch processing
            while (rawTrack.Any())
            {
                var thisBatchSize = Math.Min(GpsFilterConstants.BatchSize, rawTrack.Count);
                
                var rawTrackPortion = rawTrack
                    .Take(thisBatchSize)
                    .ToList();
                
                rawTrack.RemoveRange(0, thisBatchSize);
                
                var filteredTrackPortion = _filter.Filter(rawTrackPortion);
                
                filteredTrack.AddRange(filteredTrackPortion);
            }
            
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