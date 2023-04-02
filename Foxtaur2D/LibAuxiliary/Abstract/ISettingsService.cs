namespace LibAuxiliary.Abstract;

/// <summary>
/// Interface to work with application settings
/// </summary>
public interface ISettingsService
{
    /// <summary>
    /// Save hunters locations refresh interval to settings
    /// </summary>
    void SetHuntersLocationsRefreshInterval(double interval);

    /// <summary>
    /// Get hunters locations refresh interval from settings
    /// </summary>
    /// <returns></returns>
    double GetHuntersLocationsRefreshInterval();
}