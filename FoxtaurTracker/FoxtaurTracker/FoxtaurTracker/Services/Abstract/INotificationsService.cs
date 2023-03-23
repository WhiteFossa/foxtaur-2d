using Plugin.LocalNotification.AndroidOption;

namespace FoxtaurTracker.Services.Abstract;

/// <summary>
/// Service to send notifications
/// </summary>
public interface INotificationsService
{
    /// <summary>
    /// Show notification
    /// </summary>
    public void ShowNotification
    (
        string title,
        string text,
        int badgeCount,
        bool isSilent = false,
        AndroidPriority priority = AndroidPriority.Default,
        bool isOngoing = false
    );
}