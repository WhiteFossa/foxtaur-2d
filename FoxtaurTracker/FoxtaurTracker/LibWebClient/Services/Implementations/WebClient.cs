﻿using System.Drawing;
using LibWebClient.Constants;
using LibWebClient.Models;
using LibWebClient.Models.DTOs;
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

    public async Task<LoginResult> LoginAsync(LoginRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        CheckIfConnected();

        LoginResultDto result;

        try
        {
            result = await _client.LogInAsync(request).ConfigureAwait(false);
        }
        catch (Exception)
        {
            return new LoginResult(false, string.Empty, DateTime.MinValue);
        }

        return new LoginResult(true, result.Token, result.ExpirationTime);
    }

    public async Task SetAuthentificationTokenAsync(string token)
    {
        CheckIfConnected();

        await _client.SetAuthentificationTokenAsync(token).ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<Profile>> MassGetProfilesAsync(ProfilesMassGetRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));
        
        CheckIfConnected();

        var profiles = await _client.MassGetProfilesAsync(request).ConfigureAwait(false);

        return profiles
            .Select(p =>
            {
                return new Profile
                (
                    p.Id,
                    p.FirstName,
                    p.MiddleName,
                    p.LastName,
                    p.Sex,
                    p.DateOfBirth,
                    p.Phone,
                    p.Team != null
                        ? new Team(p.Team.Id, p.Team.Name, Color.FromArgb(p.Team.Color.A, p.Team.Color.R, p.Team.Color.G, p.Team.Color.B))
                        : null,
                    p.Category,
                    Color.FromArgb(p.Color.A, p.Color.R, p.Color.G, p.Color.B)
                );
            })
            .ToList();
    }

    public async Task<UserInfo> GetCurrentUserInfoAsync()
    {
        CheckIfConnected();

        var userInfo = await _client.GetCurrentUserInfoAsync().ConfigureAwait(false);

        return new UserInfo(userInfo.Id, userInfo.Login, userInfo.Email);
    }

    private void CheckIfConnected()
    {
        if (!_isConnected)
        {
            throw new InvalidOperationException("Not connected to server.");
        }
    }
}