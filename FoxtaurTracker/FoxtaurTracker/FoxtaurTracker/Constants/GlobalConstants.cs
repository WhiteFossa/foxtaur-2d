namespace FoxtaurTracker.Constants;

/// <summary>
/// Global constants
/// </summary>
public static class GlobalConstants
{
    /// <summary>
    /// Timeout for getting GPS coordinates
    /// </summary>
    public const int GpsTimeout = 10;

    /// <summary>
    /// Get new location each this number of seconds
    /// </summary>
    public const int LocationFetchInterval = 5;
    
    /// <summary>
    /// Try to send locations each this number of seconds
    /// </summary>
    public const int LocationsSendInterval = 1;
    
    /// <summary>
    /// Send no more than this amount of locations at once
    /// </summary>
    public const int MaxSendLocationsCount = 10;
}