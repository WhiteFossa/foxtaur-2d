using LibWebClient.Models.DTOs;
using LibWebClient.Models.Requests;

namespace LibWebClient.Services.Abstract;

/// <summary>
/// Low-level web client
/// </summary>
public interface IWebClientRaw
{
    /// <summary>
    /// Get information about the server
    /// </summary>
    Task<ServerInfoDto> GetServerInfoAsync();

    /// <summary>
    /// Register user on server.
    /// </summary>
    Task<bool> RegisterOnServerAsync(RegistrationRequest request);

    /// <summary>
    /// Log in
    /// </summary>
    Task<LoginResultDto> LogInAsync(LoginRequest request);
    
    /// <summary>
    /// Set authentification token for all subsequent queries
    /// </summary>
    Task SetAuthentificationTokenAsync(string token);

    /// <summary>
    /// Mass get hunters profiles
    /// </summary>
    Task<IReadOnlyCollection<ProfileDto>> MassGetProfilesAsync(ProfilesMassGetRequest request);
    
    /// <summary>
    /// Get information about current user
    /// </summary>
    /// <returns></returns>
    Task<UserInfoDto> GetCurrentUserInfoAsync();

    /// <summary>
    /// Get all existing teams
    /// </summary>
    Task<IReadOnlyCollection<TeamDto>> GetAllTeamsAsync();

    /// <summary>
    /// Update current user profile
    /// </summary>
    Task<ProfileDto> UpdateProfileAsync(ProfileUpdateRequest request);
    
    /// <summary>
    /// Create new team
    /// </summary>
    Task<TeamDto> CreateTeamAsync(CreateTeamRequest request);

    /// <summary>
    /// Get all distances
    /// </summary>
    Task<IReadOnlyCollection<DistanceDto>> GetAllDistancesAsync();
    
    /// <summary>
    /// Mass get maps
    /// </summary>
    Task<IReadOnlyCollection<MapDto>> MassGetMapsAsync(MapsMassGetRequest request);

    /// <summary>
    /// Register on a distance
    /// </summary>
    Task<RegistrationOnDistanceResponseDto> RegisterOnDistanceAsync(RegisterOnDistanceRequest request);
    
    /// <summary>
    /// Create hunter locations. Returns IDs of locations, which were successfully stored
    /// </summary>
    Task<IReadOnlyCollection<Guid>> CreateHunterLocationsAsync(CreateHunterLocationsRequest request);
}