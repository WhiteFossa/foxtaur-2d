using LibWebClient.Models;
using LibWebClient.Models.DTOs;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;

namespace LibWebClient.Services.Implementations;

public class WebClient : IWebClient
{
    private readonly IWebClientRaw _client;
    
    private string _login;
    private string _password;
    
    private string _token;
    private DateTime _tokenExpiration;

    public WebClient(IWebClientRaw client)
    {
        _client = client;
    }

    public async Task<ServerInfo> GetServerInfoAsync()
    {
        var serverInfo = await _client.GetServerInfoAsync();

        return new ServerInfo(serverInfo.Name, serverInfo.ProtocolVersion);
    }

    public async Task<LoginResult> LoginAsync(LoginRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));
        
        // DO NOT call RenewSessionAsync here because of recursion

        _login = request.Login;
        _password = request.Password;

        LoginResultDto result;

        try
        {
            result = await _client.LogInAsync(request).ConfigureAwait(false);
        }
        catch (Exception)
        {
            await LogoutAsync();
            
            return new LoginResult(false, string.Empty, DateTime.MinValue);
        }

        _token = result.Token;
        _tokenExpiration = result.ExpirationTime;
        
        await _client.SetAuthentificationTokenAsync(_token).ConfigureAwait(false);
        
        return new LoginResult(true, result.Token, result.ExpirationTime);
    }

    public async Task LogoutAsync()
    {
        _login = String.Empty;
        _password = string.Empty;
        _token = string.Empty;
        _tokenExpiration = DateTime.MinValue;
        await _client.SetAuthentificationTokenAsync(_token).ConfigureAwait(false);
    }
}