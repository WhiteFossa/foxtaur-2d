using FoxtaurServer.Models.Api;
using FoxtaurServer.Models.Api.Requests;

namespace FoxtaurServer.Services.Abstract;

/// <summary>
/// Service to work with hunters
/// </summary>
public interface IHuntersService
{
    /// <summary>
    /// Mass get hunters by their IDs
    /// </summary>
    Task<IReadOnlyCollection<HunterDto>> MassGetHuntersAsync(IReadOnlyCollection<Guid> huntersIds);

    /// <summary>
    /// Mass get hunters profiles
    /// </summary>
    Task<IReadOnlyCollection<ProfileDto>> MassGetHuntersProfilesAsync(IReadOnlyCollection<Guid> huntersIds);

    /// <summary>
    /// Update hunter's profile
    /// </summary>
    Task<ProfileDto> UpdateHunterProfileAsync(ProfileUpdateRequest request);

    /// <summary>
    /// Register hunter on distance
    /// </summary>
    Task<RegistrationOnDistanceResponseDto> RegisterOnDistanceAsync(RegisterOnDistanceRequest request, Guid hunterId);
}