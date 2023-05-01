using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using LibAuxiliary.Abstract;
using LibAuxiliary.Constants;
using LibWebClient.Constants;
using LibWebClient.Models.Abstract;
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
        _httpClient.Timeout = new TimeSpan(0, 0, WebClientConstants.HttpClientTimeout);

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

        return JsonSerializer.Deserialize<HuntersLocationsDictionaryDto>(await response.Content.ReadAsStringAsync()).HuntersLocations;
    }

    public async Task<IReadOnlyCollection<FoxDto>> MassGetFoxesAsync(FoxesMassGetRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Foxes/MassGet", request).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            _logger.Error($"MassGetFoxesAsync failed: { response.StatusCode }");
            throw new InvalidOperationException();
        }

        var result = JsonSerializer.Deserialize<IReadOnlyCollection<FoxDto>>(await response.Content.ReadAsStringAsync());
        return ReorderResult(result, request.FoxesIds);
    }

    public async Task<IReadOnlyCollection<TeamDto>> MassGetTeamsAsync(TeamsMassGetRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Teams/MassGet", request).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            _logger.Error($"MassGetTeamsAsync failed: { response.StatusCode }");
            throw new InvalidOperationException();
        }
        
        var result = JsonSerializer.Deserialize<IReadOnlyCollection<TeamDto>>(await response.Content.ReadAsStringAsync());
        return ReorderResult(result, request.TeamsIds);
    }

    public async Task<IReadOnlyCollection<MapDto>> MassGetMapsAsync(MapsMassGetRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Maps/MassGet", request).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            _logger.Error($"MassGetMapsAsync failed: { response.StatusCode }");
            throw new InvalidOperationException();
        }
        
        var result = JsonSerializer.Deserialize<IReadOnlyCollection<MapDto>>(await response.Content.ReadAsStringAsync());
        return ReorderResult(result, request.MapsIds);
    }

    public async Task<IReadOnlyCollection<HunterDto>> MassGetHuntersAsync(HuntersMassGetRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Hunters/MassGet", request).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            _logger.Error($"MassGetHuntersAsync failed: { response.StatusCode }");
            throw new InvalidOperationException();
        }
        
        var result = JsonSerializer.Deserialize<IReadOnlyCollection<HunterDto>>(await response.Content.ReadAsStringAsync());
        return ReorderResult(result, request.HuntersIds);
    }

    public async Task<IReadOnlyCollection<LocationDto>> MassGetLocationsAsync(LocationsMassGetRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/Locations/MassGet", request).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            _logger.Error($"MassGetLocationsAsync failed: { response.StatusCode }");
            throw new InvalidOperationException();
        }

        var result = JsonSerializer.Deserialize<IReadOnlyCollection<LocationDto>>(await response.Content.ReadAsStringAsync());
        return ReorderResult(result, request.LocationsIds);
    }

    public async Task<HttpResponseMessage> GetHeadersAsync(Uri uri)
    {
        _ = uri ?? throw new ArgumentNullException(nameof(uri));
        
        using var webRequest = new HttpRequestMessage(HttpMethod.Head, uri);
        
        var response = await _httpClient.SendAsync(webRequest);
        if (!response.IsSuccessStatusCode)
        {
            _logger.Error($"GetHeadersAsync failed: { response.StatusCode }");
            throw new InvalidOperationException();
        }

        return response;
    }

    public async Task<HttpResponseMessage> DownloadWithRangeAsync(Uri uri, long start, long end)
    {
        _ = uri ?? throw new ArgumentNullException(nameof(uri));
        
        if (start < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(start), "Start must be non-negative.");
        }

        if (end <= start)
        {
            throw new ArgumentOutOfRangeException(nameof(end), "End must be greater than start.");
        }
        
        var httpClient = new HttpClient();
        httpClient.Timeout = new TimeSpan(0, 0, WebClientConstants.HttpClientDownloadTimeout);
        using var webRequest = new HttpRequestMessage(HttpMethod.Get, uri);
        webRequest.Headers.Range = new RangeHeaderValue(start, end);

        var response = await httpClient.SendAsync(webRequest);
        if (!response.IsSuccessStatusCode)
        {
            _logger.Error($"DownloadWithRangeAsync failed: { response.StatusCode }");
            throw new InvalidOperationException();
        }

        return response;
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