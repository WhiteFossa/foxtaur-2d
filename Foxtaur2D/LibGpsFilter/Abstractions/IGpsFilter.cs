namespace LibGpsFilter.Abstractions;

public interface IGpsFilter
{
    /// <summary>
    /// Denoise list of locations.
    /// </summary>
    IReadOnlyCollection<GpsLocation> FilterLocations(IReadOnlyCollection<GpsLocation> locations);
}