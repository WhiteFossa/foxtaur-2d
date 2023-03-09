using System.Net.Http.Json;
using System.Text.Json;
using LibAuxiliary.Abstract;
using LibAuxiliary.Constants;
using LibGeo.Implementations.Helpers;
using LibWebClient.Enums;
using LibWebClient.Models.DTOs;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;
using NLog;

namespace LibWebClient.Services.Implementations;

public class WebClientRaw : IWebClientRaw
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    
    private readonly IConfigurationService _configurationService;
    
    private readonly string _baseUrl;
    
    private readonly HttpClient _httpClient;

    public WebClientRaw(IConfigurationService configurationService,
        HttpClient httpClient)
    {
        _configurationService = configurationService;
        _httpClient = httpClient;
        
        _configurationService = configurationService;

        _logger.Info($"Working with { _configurationService.GetConfigurationString(ConfigConstants.ServerUrlSettingName) } server");
        _baseUrl = _configurationService.GetConfigurationString(ConfigConstants.ServerUrlSettingName) + @"/api";
    }
    
    public async Task<ServerInfoDto> GetServerInfoAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/ServerInfo/Index").ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            _logger.Error($"GetServerInfoAsync failed: { response.StatusCode }");
            throw new InvalidOperationException();
        }
        
        return JsonSerializer.Deserialize<ServerInfoDto>(await response.Content.ReadAsStringAsync());
    }

    public async Task<TeamDto> GetTeamByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/Teams/{id}").ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            _logger.Error($"GetTeamByIdAsync failed: { response.StatusCode }");
            throw new InvalidOperationException();
        }
        
        return JsonSerializer.Deserialize<TeamDto>(await response.Content.ReadAsStringAsync());
    }

    public async Task<HunterDto> GetHunterByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/Hunters/{id}").ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            _logger.Error($"GetHunterByIdAsync failed: { response.StatusCode }");
            throw new InvalidOperationException();
        }
        
        return JsonSerializer.Deserialize<HunterDto>(await response.Content.ReadAsStringAsync());
    }

    public async Task<LocationDto> GetLocationByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/Locations/{id}").ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            _logger.Error($"GetLocationByIdAsync failed: { response.StatusCode }");
            throw new InvalidOperationException();
        }
        
        return JsonSerializer.Deserialize<LocationDto>(await response.Content.ReadAsStringAsync());
    }

    public async Task<MapDto> GetMapByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/Maps/{id}").ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            _logger.Error($"GetMapByIdAsync failed: { response.StatusCode }");
            throw new InvalidOperationException();
        }
        
        return JsonSerializer.Deserialize<MapDto>(await response.Content.ReadAsStringAsync());
    }

    public async Task<DistanceDto> GetDistanceByIdAsync(Guid id)
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/Distances/{id}").ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            _logger.Error($"GetDistanceByIdAsync failed: { response.StatusCode }");
            throw new InvalidOperationException();
        }
        
        return JsonSerializer.Deserialize<DistanceDto>(await response.Content.ReadAsStringAsync());
    }

    public async Task<IReadOnlyCollection<DistanceDto>> ListDistancesAsync()
    {
        var response = await _httpClient.GetAsync($"{_baseUrl}/Distances/Index").ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            _logger.Error($"ListDistancesAsync failed: { response.StatusCode }");
            throw new InvalidOperationException();
        }
        
        return JsonSerializer.Deserialize<IReadOnlyCollection<DistanceDto>>(await response.Content.ReadAsStringAsync());
    }

    public async Task<Dictionary<Guid, IReadOnlyCollection<HunterLocationDto>>> MassGetHuntersLocationsAsync(HuntersLocationsMassGetRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/HuntersLocations/MassGet", request).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            _logger.Error($"MassGetHuntersLocationsAsync failed: { response.StatusCode }");
            throw new InvalidOperationException();
        }
        
        return JsonSerializer.Deserialize<Dictionary<Guid, IReadOnlyCollection<HunterLocationDto>>>(await response.Content.ReadAsStringAsync());
    }

    public async Task<IReadOnlyCollection<FoxDto>> MassGetFoxesAsync(FoxesMassGetRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Foxes/MassGet", request).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            _logger.Error($"MassGetFoxesAsync failed: { response.StatusCode }");
            throw new InvalidOperationException();
        }
        
        return JsonSerializer.Deserialize<IReadOnlyCollection<FoxDto>>(await response.Content.ReadAsStringAsync());
    }
}