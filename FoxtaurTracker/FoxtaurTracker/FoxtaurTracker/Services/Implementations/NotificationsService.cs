using FoxtaurTracker.Constants;
using FoxtaurTracker.Services.Abstract;
using Plugin.LocalNotification;
using Plugin.LocalNotification.AndroidOption;

namespace FoxtaurTracker.Services.Implementations;

public class NotificationsService : INotificationsService
{
    public void ShowNotification
    (
        string title,
        string text,
        int badgeCount,
        bool isSilent = false,
        AndroidPriority priority = AndroidPriority.Default,
        bool isOngoing = false
    )
    {
        var request = new NotificationRequest
        {
            NotificationId = GlobalConstants.NotificationId,
            Title = title,
            Description = text,
            BadgeNumber = badgeCount,
            Silent = isSilent,
            Android = new AndroidOptions()
            {
                Priority = priority,
                Ongoing = isOngoing
            }
        };
        
        LocalNotificationCenter.Current.Show(request);
    }
}