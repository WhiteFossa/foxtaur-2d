using FoxtaurServer.Models.Api.Requests;

namespace FoxtaurServer.Services.Abstract.Models;

/// <summary>
/// Result of login method
/// </summary>
public class LoginResult
{
    /// <summary>
    /// Is credentials correct?
    /// </summary>
    public bool IsSuccessful { get; }

    /// <summary>
    /// Token
    /// </summary>
    public string Token { get; }

    /// <summary>
    /// Token expiration date and time
    /// </summary>
    public DateTime ExpirationTime { get; }

    public LoginResult(bool isSuccessful, string token, DateTime expirationTime)
    {
        IsSuccessful = isSuccessful;
        Token = token;
        ExpirationTime = expirationTime;
    }
}