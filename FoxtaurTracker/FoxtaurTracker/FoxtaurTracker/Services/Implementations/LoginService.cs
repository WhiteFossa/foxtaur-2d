using FoxtaurTracker.Models;
using FoxtaurTracker.Services.Abstract;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;

namespace FoxtaurTracker.Services.Implementations;

public class LoginService : ILoginService
{
    private readonly IWebClient _webClient;
    private readonly ISettingsService _settingsService;

    public LoginService(IWebClient webClient, ISettingsService settingsService)
    {
        _webClient = webClient;
        _settingsService = settingsService;
    }
    
    public async Task<Tuple<bool, User>> LogInAsync(string login, string password, bool isRememberMe)
    {
        var request = new LoginRequest(login, password);
        var result = await _webClient.LoginAsync(request); // Web client handles exceptions

        if (!result.IsSuccessful)
        {
            await RemoveStoredCredentialsAsync();

            return new Tuple<bool, User>(false, new User()
            {
                Login = login
            });
        }
            
        // We are successfully logged in
        if (isRememberMe)
        {
            _settingsService.SaveLogin(login);
            await _settingsService.SavePasswordAsync(password);
        }
        
        var user = new User();

        return new Tuple<bool, User>(true, user);
    }

    public async Task<Tuple<bool, User>> TryPerformAutologinAsync()
    {
        // Do we have stored credentials for auto-login?
        if (!_settingsService.GetLogin(out var login))
        {
            return new Tuple<bool, User>(false, new User()
            {
                Login = login
            });
        }

        var password = await _settingsService.GetPasswordAsync();
        if (password == null)
        {
            return new Tuple<bool, User>(false, new User()
            {
                Login = login
            });
        }

        var loginResult = await LogInAsync(login, password, false); // False - because credentials are already stored

        if (!loginResult.Item1)
        {
            // Autologin credentials didn't work
            await MainThread.InvokeOnMainThreadAsync(async () =>
                await App.PopupsService.ShowAlertAsync("Warning", "Server rejected saved credentials. You need to log in manually."));
        }
        
        return loginResult;
    }

    public async Task LogOutAsync()
    {
        await RemoveStoredCredentialsAsync();
        await _webClient.LogoutAsync();
    }

    private async Task RemoveStoredCredentialsAsync()
    {
        await _settingsService.RemovePasswordIfExistAsync();
        _settingsService.RemoveLoginIfExist();
    }
}