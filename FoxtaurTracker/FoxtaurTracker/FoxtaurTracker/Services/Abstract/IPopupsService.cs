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

    /// <summary>
    /// Ask a question to user
    /// </summary>
    Task<bool> ShowQuestionAsync(string title, string message, string yesButton = "Yes", string noButton = "No");
}