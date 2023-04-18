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
}