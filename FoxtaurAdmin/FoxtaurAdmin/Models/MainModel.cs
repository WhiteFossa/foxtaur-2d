using LibWebClient.Models;

namespace FoxtaurAdmin.Models;

/// <summary>
/// Main model
/// </summary>
public class MainModel
{
    /// <summary>
    /// Information about current server
    /// </summary>
    public ServerInfo ServerInfo { get; set; }

    /// <summary>
    /// If true, then user logged in
    /// </summary>
    public bool IsLoggedIn { get; set; }
}