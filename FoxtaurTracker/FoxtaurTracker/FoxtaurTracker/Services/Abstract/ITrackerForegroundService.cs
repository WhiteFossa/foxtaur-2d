namespace FoxtaurTracker.Services.Abstract;

public interface ITrackerForegroundService
{
    /// <summary>
    /// Display new notification
    /// </summary>
    void SendNewNotification(string content);
}