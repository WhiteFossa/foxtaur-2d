namespace LibWebClient.Models;

/// <summary>
/// Information about logged-in user
/// </summary>
public class UserInfo
{
    /// <summary>
    /// Distance ID
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Login
    /// </summary>
    public string Login { get; }
    
    /// <summary>
    /// Email
    /// </summary>
    public string Email { get; }

    public UserInfo(Guid id, string login, string email)
    {
        if (string.IsNullOrWhiteSpace(login))
        {
            throw new ArgumentException(nameof(login));
        }
        
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException(nameof(email));
        }
        
        Id = id;
        Login = login;
        Email = email;
    }
}