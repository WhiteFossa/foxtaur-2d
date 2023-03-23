using FoxtaurTracker.Models;

namespace FoxtaurTracker.Services.Abstract;

/// <summary>
/// Service to log in / log out
/// </summary>
public interface ILoginService
{
    /// <summary>
    /// Log user in, saving hir credentials if log in successful and isRememberMe is true.
    /// If credentials isn't correct - deletes stored credentials
    /// Returns true as first item if login successful
    /// </summary>
    Task<Tuple<bool, User>> LogInAsync(string login, string password, bool isRememberMe);

    /// <summary>
    /// Try to perform autologin. Returns true and user data if successful
    /// </summary>
    Task<Tuple<bool, User>> TryPerformAutologinAsync();
    
    /// <summary>
    /// Log user out
    /// </summary>
    Task LogOutAsync();
}