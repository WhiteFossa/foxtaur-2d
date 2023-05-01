using System.Text.Json;
using LibAuxiliary.Abstract;
using LibAuxiliary.Constants;
using LibWebClient.Constants;
using LibWebClient.Models.DTOs;
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
}