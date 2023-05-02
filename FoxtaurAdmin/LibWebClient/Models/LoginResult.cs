namespace LibWebClient.Models;

/// <summary>
/// Login result
/// </summary>
public class LoginResult
{
    /// <summary>
    /// Is successful
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