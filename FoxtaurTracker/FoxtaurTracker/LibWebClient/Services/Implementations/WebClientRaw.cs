﻿using System.Net.Http.Json;
using System.Text.Json;
using LibWebClient.Constants;
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
}