using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using FoxtaurTracker.Constants;

namespace FoxtaurTracker;

[Service]
public class TrackerForegroundService : Service
{
    private void StartForegroundService()
    {
        var notifcationManager = GetSystemService(NotificationService) as NotificationManager;

        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            CreateNotificationChannel(notifcationManager);
        }

        var notification = new NotificationCompat.Builder(this, GlobalConstants.NotificationChannelId);
        
        notification.SetAutoCancel(false);
        notification.SetOngoing(true);
        
        notification.SetSmallIcon(Resource.Mipmap.appicon);
        notification.SetContentTitle(GlobalConstants.TrackingIsOnNotificationTitle);
        notification.SetContentText("Starting up..."); // TODO: Localize me
        
        StartForeground(GlobalConstants.NotificationId, notification.Build());
    }

    private void CreateNotificationChannel(NotificationManager notificationMnaManager)
    {
        var channel = new NotificationChannel(GlobalConstants.NotificationChannelId, GlobalConstants.NotificationChannelName, NotificationImportance.Low);
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