namespace LibAuxiliary.Helpers;

/// <summary>
/// Usefull stuff to work with time
/// </summary>
public static class TimeHelper
{
    /// <summary>
    /// Converts nullable timespan to something like "01:23:45 ago" or "N/A" if null
    /// </summary>
    public static string AsTimeAgo(this TimeSpan? timespan)
    {
        return timespan.HasValue ? $"{timespan.Value:hh\\:mm\\:ss} ago" : "N/A";
    }
}