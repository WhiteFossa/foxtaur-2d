using System.Drawing;
using LibWebClient.Constants;
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

    public async Task<MapFile> CreateMapFileAsync(CreateMapFileRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));
        await RenewSessionAsync();

        var createdMapFile = await _client.CreateMapFileAsync(request).ConfigureAwait(false);

        return new MapFile
        (
            createdMapFile.Id,
            createdMapFile.Name
        );
    }

    public async Task UploadMapFilePartAsync(UploadMapFilePartRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));
        await RenewSessionAsync();

        await _client.UploadMapFilePartAsync(request).ConfigureAwait(false);
    }

    public async Task MarkMapFileAsReadyAsync(MarkMapFileAsReadyRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));
        await RenewSessionAsync();

        await _client.MarkMapFileAsReadyAsync(request).ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<MapFile>> GetAllMapFilesAsync()
    {
        await RenewSessionAsync();

        var mapFiles = await _client.GetAllMapFilesAsync().ConfigureAwait(false);

        return mapFiles
            .Select(mf => new MapFile(mf.Id, mf.Name))
            .ToList();
    }

    public async Task<Map> CreateMapAsync(CreateMapRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));
        await RenewSessionAsync();

        var createdMap = await _client.CreateMapAsync(request).ConfigureAwait(false);

        return new Map
        (
            createdMap.Id,
            createdMap.Name,
            createdMap.NorthLat,
            createdMap.SouthLat,
            createdMap.EastLon,
            createdMap.WestLon,
            createdMap.FileId
        );
    }

    private async Task RenewSessionAsync()
    {
        if (string.IsNullOrWhiteSpace(_token))
        {
            // We aren't logged in
            return;
        }

        var remaining = _tokenExpiration - DateTime.UtcNow;
        if (remaining > WebClientConstants.ReauthentificateBefore)
        {
            return;
        }

        await LoginAsync(new LoginRequest(_login, _password));
    }
}