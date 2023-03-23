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
    public const int LocationFetchInterval = 2;
    
    /// <summary>
    /// Minimal shift for what new location event will be generated
    /// </summary>
    public const double LocationFetchMinimalDistance = 5.0;
    
    /// <summary>
    /// Try to send locations each this number of seconds
    /// </summary>
    public const int LocationsSendInterval = 1;
    
    /// <summary>
    /// Send no more than this amount of locations at once
    /// </summary>
    public const int MaxSendLocationsCount = 10;

    #region Notifications
    
    /// <summary>
    /// Unique notification ID
    /// </summary>
    public const int NotificationId = 40578;
    
    /// <summary>
    /// Notification channel ID
    /// </summary>
    public const string NotificationChannelId = "me.foxtaur.tracker";
    
    /// <summary>
    /// Notification channel name
    /// </summary>
    public const string NotificationChannelName = "me.foxtaur.tracker_notifications";

    /// <summary>
    /// Title for "tracking is on" notification
    /// </summary>
    public const string TrackingIsOnNotificationTitle = "Tracking is ON";

    /// <summary>
    /// Update tracking notification each this number of seconds
    /// </summary>
    public const int NotificationUpdateInterval = 1;

    #endregion
}