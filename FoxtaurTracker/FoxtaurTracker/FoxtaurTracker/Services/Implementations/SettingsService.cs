using FoxtaurTracker.Services.Abstract;

namespace FoxtaurTracker.Services.Implementations;

public class SettingsService : ISettingsService
{
    private const string LoginKey = "login";
    private const string PasswordKey = "password";
    
    public void SaveLogin(string login)
    {
        Preferences.Default.Set("login", login);
    }

    public bool GetLogin(out string login)
    {
        if (!Preferences.Default.ContainsKey(LoginKey))
        {
            login = string.Empty;
            return false;
        }
        
        login = Preferences.Default.Get(LoginKey, string.Empty);
        return true;
    }

    public void RemoveLoginIfExist()
    {
        Preferences.Default.Remove(LoginKey);
    }

    public async Task SavePasswordAsync(string password)
    {
        await SecureStorage.Default.SetAsync(PasswordKey, password);
    }

    public async Task<string> GetPasswordAsync()
    {
        return await SecureStorage.Default.GetAsync(PasswordKey);
    }
    
    public async Task RemovePasswordIfExistAsync()
    {
        SecureStorage.Default.Remove(PasswordKey);
    }
}