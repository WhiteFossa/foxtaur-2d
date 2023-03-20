using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;

namespace FoxtaurTracker;

[Service]
public class TrackerForegroundService : Service
{
    private const string NotificationChannelId = "me.foxtaur.tracker";
    private const int NotificationId = 40578;
    private const string NotificationChannelName = "me.foxtaur.tracker_notifications";

    private void StartForegroundService()
    {
        var notifcationManager = GetSystemService(NotificationService) as NotificationManager;

        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            CreateNotificationChannel(notifcationManager);
        }

        var notification = new NotificationCompat.Builder(this, NotificationChannelId);
        
        notification.SetAutoCancel(false);
        notification.SetOngoing(true);
        
        notification.SetSmallIcon(Resource.Mipmap.appicon);
        notification.SetContentTitle("Foxtaur Tracker");
        notification.SetContentText("Tracking is ON");
        
        StartForeground(NotificationId, notification.Build());
    }

    private void CreateNotificationChannel(NotificationManager notificationMnaManager)
    {
        var channel = new NotificationChannel(NotificationChannelId, NotificationChannelName, NotificationImportance.Low);
        notificationMnaManager.CreateNotificationChannel(channel);
    }

    public override IBinder OnBind(Intent intent)
    {
        return null;
    }
    
    public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
    {
        StartForegroundService();
        return StartCommandResult.NotSticky;
    }
}