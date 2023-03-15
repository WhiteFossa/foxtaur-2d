using LibWebClient.Constants;
using LibWebClient.Models;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;

namespace LibWebClient.Services.Implementations;

public class WebClient : IWebClient
{
    private readonly IWebClientRaw _client;

    private bool _isConnected;
    
    public WebClient(IWebClientRaw webClientRaw)
    {
        _client = webClientRaw;
    }

    public async Task ConnectAsync()
    {
        // Querying information about the server
        var serverInfo = await _client.GetServerInfoAsync();

        if (WebClientConstants.ProtocolVersion != serverInfo.ProtocolVersion)
        {
            throw new InvalidOperationException("Protocol version mismatch.");
        }

        _isConnected = true;
    }

    public async Task<ServerInfo> GetServerInfoAsync()
    {
        var serverInfo = await _client.GetServerInfoAsync();

        return new ServerInfo(serverInfo.Name, serverInfo.ProtocolVersion);
    }

    public async Task<bool> RegisterUserAsync(RegistrationRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        CheckIfConnected();

        return await _client.RegisterOnServerAsync(request).ConfigureAwait(false);
    }

    private void CheckIfConnected()
    {
        if (!_isConnected)
        {
            throw new InvalidOperationException("Not connected to server.");
        }
    }
}