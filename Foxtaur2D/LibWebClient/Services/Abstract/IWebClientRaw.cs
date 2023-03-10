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
    /// <returns></returns>
    Task<ServerInfoDto> GetServerInfoAsync();
    
    /// <summary>
    /// Get location by ID. Throws ArgumentException if location with given ID is not found
    /// </summary>
    Task<LocationDto> GetLocationByIdAsync(Guid id);
    
    /// <summary>
    /// Get distance by ID. Throws ArgumentException if map with given ID is not found
    /// </summary>
    Task<DistanceDto> GetDistanceByIdAsync(Guid id);

    /// <summary>
    /// Lists all available distances
    /// </summary>
    Task<IReadOnlyCollection<DistanceDto>> ListDistancesAsync();
    
    /// <summary>
    /// Mass get hunters locations
    /// </summary>
    Task<Dictionary<Guid, IReadOnlyCollection<HunterLocationDto>>> MassGetHuntersLocationsAsync(HuntersLocationsMassGetRequest request);

    /// <summary>
    /// Mass get foxes
    /// </summary>
    Task<IReadOnlyCollection<FoxDto>> MassGetFoxesAsync(FoxesMassGetRequest request);

    /// <summary>
    /// Mass get teams
    /// </summary>
    Task<IReadOnlyCollection<TeamDto>> MassGetTeamsAsync(TeamsMassGetRequest request);
    
    /// <summary>
    /// Mass get maps
    /// </summary>
    Task<IReadOnlyCollection<MapDto>> MassGetMapsAsync(MapsMassGetRequest request);
    
    /// <summary>
    /// Mass get hunters
    /// </summary>
    Task<IReadOnlyCollection<HunterDto>> MassGetHuntersAsync(HuntersMassGetRequest request);
}