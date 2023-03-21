namespace LibGpsFilter.Constants;

/// <summary>
/// Constants for GPS filter
/// </summary>
public static class GpsFilterConstants
{
    /// <summary>
    /// Minimal possible hunter speed, km/h 
    /// </summary>
    public const double MinHunterSpeed = 0.0;
    
    /// <summary>
    /// Maximal possible hunter speed, km/h 
    /// </summary>
    public const double MaxHunterSpeed = 30.0;

    /// <summary>
    /// Process no more than this number of coordinates per pass
    /// </summary>
    public const int BatchSize = 500;
}