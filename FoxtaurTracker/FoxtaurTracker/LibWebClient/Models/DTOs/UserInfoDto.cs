﻿using System.Text.Json.Serialization;

namespace LibWebClient.Models.DTOs;

/// <summary>
/// Information about current user
/// </summary>
public class UserInfoDto
{
    /// <summary>
    /// Distance ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Login
    /// </summary>
    [JsonPropertyName("login")]
    public string Login { get; set; }
    
    /// <summary>
    /// Email
    /// </summary>
    [JsonPropertyName("email")]
    public string Email { get; set; }
}