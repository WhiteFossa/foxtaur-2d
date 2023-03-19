using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using LibWebClient.Constants;
using LibWebClient.Models.Abstract;
using LibWebClient.Models.DTOs;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;

namespace LibWebClient.Services.Implementations;

public class WebClientRaw : IWebClientRaw
{
    private readonly string _baseUrl;

    private readonly HttpClient _httpClient;

    public WebClientRaw()
    {
        _httpClient = new HttpClient();
        _httpClient.Timeout = new TimeSpan(0, 0, WebClientConstants.HttpClientTimeout);
        
        _baseUrl = @"https://api.foxtaur.me" + @"/api"; // TODO: Read from config
    }

    public async Task<ServerInfoDto> GetServerInfoAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/ServerInfo/Index").ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
        
        return JsonSerializer.Deserialize<ServerInfoDto>(await response.Content.ReadAsStringAsync());
    }

    public async Task<bool> RegisterOnServerAsync(RegistrationRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));
        
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Accounts/Register", request).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            return false;
        }

        return true;
    }

    public async Task<LoginResultDto> LogInAsync(LoginRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));
        
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Accounts/Login", request).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }

        return JsonSerializer.Deserialize<LoginResultDto>(await response.Content.ReadAsStringAsync());
    }
    
    public async Task SetAuthentificationTokenAsync(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    public async Task<IReadOnlyCollection<ProfileDto>> MassGetProfilesAsync(ProfilesMassGetRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));
        
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Hunters/Profiles/MassGet", request).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }

        var result = JsonSerializer.Deserialize<IReadOnlyCollection<ProfileDto>>(await response.Content.ReadAsStringAsync());
        return ReorderResult(result, request.HuntersIds);
    }

    public async Task<UserInfoDto> GetCurrentUserInfoAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/Accounts/GetCurrentUserInformation").ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
        
        return JsonSerializer.Deserialize<UserInfoDto>(await response.Content.ReadAsStringAsync());
    }

    public async Task<IReadOnlyCollection<TeamDto>> GetAllTeamsAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/Teams/GetAll").ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
        
        return JsonSerializer.Deserialize<IReadOnlyCollection<TeamDto>>(await response.Content.ReadAsStringAsync());
    }

    public async Task<ProfileDto> UpdateProfileAsync(ProfileUpdateRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Hunters/Profiles/Update", request).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }

        return JsonSerializer.Deserialize<ProfileDto>(await response.Content.ReadAsStringAsync());
    }

    public async Task<TeamDto> CreateTeamAsync(CreateTeamRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Teams/Create", request).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }

        return JsonSerializer.Deserialize<TeamDto>(await response.Content.ReadAsStringAsync());
    }

    public async Task<IReadOnlyCollection<DistanceDto>> GetAllDistancesAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/Distances/Index").ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
        
        return JsonSerializer.Deserialize<IReadOnlyCollection<DistanceDto>>(await response.Content.ReadAsStringAsync());
    }

    public async Task<IReadOnlyCollection<MapDto>> MassGetMapsAsync(MapsMassGetRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Maps/MassGet", request).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }

        var result = JsonSerializer.Deserialize<IReadOnlyCollection<MapDto>>(await response.Content.ReadAsStringAsync());
        return ReorderResult(result, request.MapsIds);
    }

    public async Task<RegistrationOnDistanceResponseDto> RegisterOnDistanceAsync(RegisterOnDistanceRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Hunters/RegisterOnDistance", request).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }

        return JsonSerializer.Deserialize<RegistrationOnDistanceResponseDto>(await response.Content.ReadAsStringAsync());
    }

    /// <summary>
    /// Reorder request result such way, that result items are orderer as request IDs
    /// </summary>
    private IReadOnlyCollection<T> ReorderResult<T>(IReadOnlyCollection<T> result, IReadOnlyCollection<Guid> requestOrder) where T : IIdedDto
    {
        var resultAsList = result
            .ToList();
        
        return requestOrder
            .Select(x => resultAsList.Single(r => r.Id == x))
            .ToList();
    }
}