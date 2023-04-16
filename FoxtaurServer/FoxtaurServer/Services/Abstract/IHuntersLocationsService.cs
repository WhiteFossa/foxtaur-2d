using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Services.Abstract;

/// <summary>
/// Service to work with hunters locations
/// </summary>
public interface IHuntersLocationsService
{
    /// <summary>
    /// Get hunters locations dictionary, where key is hunter id from huntersIds, and value is locations history (ordered) starting from fromTime
    /// </summary>
    Task<Dictionary<Guid, IReadOnlyCollection<HunterLocationDto>>> MassGetHuntersLocationsAsync(IReadOnlyCollection<Guid> huntersIds,
        DateTime fromTime,
        DateTime toTime);
    
    /// <summary>
    /// Mass add locations to given hunter. Returns successfully created (or updated) locations IDs
    /// </summary>
    Task<IReadOnlyCollection<Guid>> MassCreateHuntersLocationsAsync(IReadOnlyCollection<HunterLocationDto> huntersLocations, Guid hunterId);

    /// <summary>
    /// Add hunter location from hardware GSM-GPS tracker
    /// </summary>
    Task CreateHunterLocationFromGsmGpsTracker(string imei, DateTime time, double lat, double lon);
}