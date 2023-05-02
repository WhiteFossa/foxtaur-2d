using MessageBox.Avalonia.Enums;

namespace LibFoxtaurAdmin.Services.Abstract;

/// <summary>
/// Service to send messages to user
/// </summary>
public interface IUserMessagesService
{
    /// <summary>
    /// Show message to user
    /// </summary>
    Task ShowMessageAsync(string title, string text, Icon icon);
}