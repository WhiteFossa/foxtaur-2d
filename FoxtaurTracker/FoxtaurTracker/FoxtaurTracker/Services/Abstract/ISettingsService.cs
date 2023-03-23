namespace FoxtaurTracker.Services.Abstract;

/// <summary>
/// Service to work with settings
/// </summary>
public interface ISettingsService
{
    #region Login

    /// <summary>
    /// Save login to settings storage
    /// </summary>
    void SaveLogin(string login);

    /// <summary>
    /// Get saved login. Returns true if saved login found
    /// </summary>
    bool GetLogin(out string login);

    /// <summary>
    /// Remove record about login if it exist
    /// </summary>
    void RemoveLoginIfExist();

    #endregion
    
    #region Password
    
    /// <summary>
    /// Save password to settings storage
    /// </summary>
    Task SavePasswordAsync(string password);

    /// <summary>
    /// Get saved password. Returns null if password is not found
    /// </summary>
    Task<string> GetPasswordAsync();

    /// <summary>
    /// Remove record about password if it exist
    /// </summary>
    Task RemovePasswordIfExistAsync();
    
    #endregion
}