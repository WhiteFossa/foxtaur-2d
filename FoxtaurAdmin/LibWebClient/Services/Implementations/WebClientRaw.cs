using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using LibAuxiliary.Abstract;
using LibAuxiliary.Constants;
using LibWebClient.Constants;
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

    public WebClientRaw
    (
        IConfigurationService configurationService,
        HttpClient httpClient
    )
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

    public async Task SetAuthentificationTokenAsync(string token)
    {
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
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

    public async Task<MapFileDto> CreateMapFileAsync(CreateMapFileRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/MapFiles/Create", request).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }

        return JsonSerializer.Deserialize<MapFileDto>(await response.Content.ReadAsStringAsync());
    }

    public async Task UploadMapFilePartAsync(UploadMapFilePartRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/MapFiles/UploadPart", request).ConfigureAwait(false);
        if (!response.IsSuccessStatusCode)
        {
            throw new InvalidOperationException();
        }
    }
}