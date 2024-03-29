﻿using System.Drawing;
using LibWebClient.Constants;
using LibWebClient.Models;
using LibWebClient.Models.DTOs;
using LibWebClient.Models.Enums;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;

namespace LibWebClient.Services.Implementations;

public class WebClient : IWebClient
{
    private readonly IWebClientRaw _client;

    private bool _isConnected;

    private string _login;
    private string _password;
    
    private string _token;
    private DateTime _tokenExpiration;
    
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
        await RenewSessionAsync();

        return await _client.RegisterOnServerAsync(request).ConfigureAwait(false);
    }

    public async Task<LoginResult> LoginAsync(LoginRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        CheckIfConnected();
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

    public async Task<IReadOnlyCollection<Profile>> MassGetProfilesAsync(ProfilesMassGetRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));
        
        CheckIfConnected();
        await RenewSessionAsync();

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
        await RenewSessionAsync();

        var userInfo = await _client.GetCurrentUserInfoAsync().ConfigureAwait(false);

        return new UserInfo(userInfo.Id, userInfo.Login, userInfo.Email);
    }

    public async Task<IReadOnlyCollection<Team>> GetAllTeamsAsync()
    {
        CheckIfConnected();
        await RenewSessionAsync();

        var teams = await _client.GetAllTeamsAsync().ConfigureAwait(false);

        return teams
            .Select(t => new Team(t.Id, t.Name, Color.FromArgb(t.Color.A, t.Color.R, t.Color.G, t.Color.B)))
            .ToList();
    }

    public async Task<Profile> UpdateProfileAsync(ProfileUpdateRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));
        CheckIfConnected();
        await RenewSessionAsync();

        var updatedProfile = await _client.UpdateProfileAsync(request).ConfigureAwait(false);

        return new Profile(
            updatedProfile.Id,
            updatedProfile.FirstName,
            updatedProfile.MiddleName,
            updatedProfile.LastName,
            updatedProfile.Sex,
            updatedProfile.DateOfBirth,
            updatedProfile.Phone,
            updatedProfile.Team != null
                ? new Team(updatedProfile.Team.Id, updatedProfile.Team.Name, Color.FromArgb(updatedProfile.Team.Color.A, updatedProfile.Team.Color.R, updatedProfile.Team.Color.G, updatedProfile.Team.Color.B))
                : null,
            updatedProfile.Category,
            Color.FromArgb(updatedProfile.Color.A, updatedProfile.Color.R, updatedProfile.Color.G, updatedProfile.Color.B));
    }

    public async Task<Team> CreateTeamAsync(CreateTeamRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));
        CheckIfConnected();
        await RenewSessionAsync();

        var createdTeam = await _client.CreateTeamAsync(request).ConfigureAwait(false);

        return new Team(
            createdTeam.Id,
            createdTeam.Name,
            Color.FromArgb(createdTeam.Color.A, createdTeam.Color.R, createdTeam.Color.G, createdTeam.Color.B));
    }

    public async Task<IReadOnlyCollection<Distance>> GetDistancesWithoutIncludeAsync()
    {
        CheckIfConnected();
        await RenewSessionAsync();
        
        var distances = await _client.GetAllDistancesAsync().ConfigureAwait(false);

        var mapsIds = distances
            .Select(d => d.MapId)
            .Distinct()
            .ToList();
        var maps = await _client.MassGetMapsAsync(new MapsMassGetRequest(mapsIds)).ConfigureAwait(false);
        
        return distances
            .Select(d =>
            {
                var mapDto = maps.Single(m => m.Id == d.MapId);
                
                return new Distance(
                    d.Id,
                    d.Name,
                    new Map(mapDto.Id, mapDto.Name, mapDto.NorthLat, mapDto.SouthLat, mapDto.EastLon, mapDto.WestLon, mapDto.Url),
                    d.IsActive,
                    new Location(Guid.NewGuid(), "Invalid start location", LocationType.Start, 0, 0, null),
                    new Location(Guid.NewGuid(), "Invalid finish corridor entrance location", LocationType.FinishCorridorEntrance, 0, 0, null),
                    new Location(Guid.NewGuid(), "Invalid finish location", LocationType.Start, 0, 0, null),
                    new List<Location>(),
                    new List<Hunter>(),
                    d.FirstHunterStartTime,
                    d.CloseTime
                );
            })
            .ToList();
    }

    public async Task<RegisterOnDistanceResponse> RegisterOnDistanceAsync(RegisterOnDistanceRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));
        CheckIfConnected();
        await RenewSessionAsync();
        
        var registrationResponse = await _client.RegisterOnDistanceAsync(request).ConfigureAwait(false);

        return new RegisterOnDistanceResponse(registrationResponse.Result);
    }

    public async Task<IReadOnlyCollection<Guid>> CreateHunterLocationsAsync(CreateHunterLocationsRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));
        CheckIfConnected();
        await RenewSessionAsync();

        return await _client.CreateHunterLocationsAsync(request).ConfigureAwait(false);
    }

    public async Task<IReadOnlyCollection<GsmGpsTracker>> GetAllGsmGpsTrackersAsync()
    {
        CheckIfConnected();
        await RenewSessionAsync();

        var trackers = await _client.GetAllGsmGpsTrackersAsync().ConfigureAwait(false);

        return trackers
            .Select(t => new GsmGpsTracker(t.Id, t.Imei, t.Name, t.UsedBy))
            .ToList();
    }

    public async Task<GsmGpsTracker> ClaimGsmGpsTrackerAsync(ClaimGsmGpsTrackerRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));
        CheckIfConnected();
        await RenewSessionAsync();

        var claimedTracker = await _client.ClaimGsmGpsTrackerAsync(request).ConfigureAwait(false);
        return new GsmGpsTracker(claimedTracker.Id, claimedTracker.Imei, claimedTracker.Name, claimedTracker.UsedBy);
    }

    public async Task DeleteGsmGpsTrackerAsync(Guid trackerId)
    {
        CheckIfConnected();
        await RenewSessionAsync();

        await _client.DeleteGsmGpsTrackerAsync(trackerId).ConfigureAwait(false);
    }

    public async Task<GsmGpsTracker> CreateGsmGpsTrackerAsync(CreateGsmGpsTrackerRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));
        CheckIfConnected();
        await RenewSessionAsync();

        var tracker = await _client.CreateGsmGpsTrackerAsync(request).ConfigureAwait(false);
        return new GsmGpsTracker(tracker.Id, tracker.Imei, tracker.Name, tracker.UsedBy);
    }

    private void CheckIfConnected()
    {
        if (!_isConnected)
        {
            throw new InvalidOperationException("Not connected to server.");
        }
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