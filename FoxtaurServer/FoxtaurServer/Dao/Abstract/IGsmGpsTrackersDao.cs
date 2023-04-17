using FoxtaurServer.Dao.Models;

namespace FoxtaurServer.Dao.Abstract;

/// <summary>
/// DAO to work with GSM-interfaced trackers
/// </summary>
public interface IGsmGpsTrackersDao
{
    #region Gets

    /// <summary>
    /// Get all existing trackers
    /// </summary>
    Task<IReadOnlyCollection<GsmGpsTracker>> GetAllTrackersAsync();

    /// <summary>
    /// Get tracker by Id
    /// </summary>
    Task<GsmGpsTracker> GetByIdAsync(Guid trackerId);

    /// <summary>
    /// Get tracker by IMEI
    /// </summary>
    Task<GsmGpsTracker> GetByImeiAsync(string imei);

    #endregion
    
    #region Create / Update

    /// <summary>
    /// Create new tracker
    /// </summary>
    Task CreateAsync(GsmGpsTracker tracker);

    /// <summary>
    /// Update information about tracker
    /// </summary>
    Task UpdateAsync(GsmGpsTracker tracker);

    #endregion
}