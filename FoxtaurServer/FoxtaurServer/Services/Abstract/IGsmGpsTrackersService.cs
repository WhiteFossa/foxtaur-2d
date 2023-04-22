using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Services.Abstract;

/// <summary>
/// Service to work with GSM-interfaced GPS trackers
/// </summary>
public interface IGsmGpsTrackersService
{
    /// <summary>
    /// Get all existing trackers
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyCollection<GsmGpsTrackerDto>> GetAllTrackersAsync();

    /// <summary>
    /// Create new tracker
    /// </summary>
    Task<GsmGpsTrackerDto> CreateNewTrackerAsync(GsmGpsTrackerDto tracker);

    /// <summary>
    /// Associate the tracker with the given user. Returns null if user or tracker is not found
    /// </summary>
    Task<GsmGpsTrackerDto> ClaimTrackerAsync(Guid userId, Guid trackerId);

    /// <summary>
    /// Delete tracker by ID.
    /// </summary>
    Task DeleteTrackerAsync(Guid trackerId);
}