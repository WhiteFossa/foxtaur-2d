namespace FoxtaurTracker.Services.Abstract;

/// <summary>
/// Interface to show user popups
/// </summary>
public interface IPopupsService
{
    /// <summary>
    /// Show user an alert message
    /// </summary>
    Task ShowAlertAsync(string title, string message, string cancel = "OK");
}