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
    /// Gets team by ID. Throws ArgumentException if team with given ID is not found
    /// </summary>
    Task<TeamDto> GetTeamByIdAsync(Guid id);

    /// <summary>
    /// Gets hunter by ID. Throws ArgumentException if hunter with given ID is not found
    /// </summary>
    Task<HunterDto> GetHunterByIdAsync(Guid id);

    /// <summary>
    /// Get fox by ID. Throws ArgumentException if fox with given ID is not found
    /// </summary>
    Task<FoxDto> GetFoxByIdAsync(Guid id);

    /// <summary>
    /// Get location by ID. Throws ArgumentException if location with given ID is not found
    /// </summary>
    Task<LocationDto> GetLocationByIdAsync(Guid id);

    /// <summary>
    /// Get map by ID. Throws ArgumentException if map with given ID is not found
    /// </summary>
    Task<MapDto> GetMapByIdAsync(Guid id);

    /// <summary>
    /// Get distance by ID. Throws ArgumentException if map with given ID is not found
    /// </summary>
    Task<DistanceDto> GetDistanceByIdAsync(Guid id);

    /// <summary>
    /// Lists all available distances
    /// </summary>
    Task<IReadOnlyCollection<DistanceDto>> ListDistancesAsync();

    /// <summary>
    /// Get hunter locations history for hunter with given ID, starting from FromTime
    /// Locations are ordered from old to new
    /// </summary>
    Task<IReadOnlyCollection<HunterLocationDto>> GetHunterLocationsHistoryAsync(Guid id, DateTime fromTime);
    
    /// <summary>
    /// Mass get hunters locations
    /// </summary>
    Task<Dictionary<Guid, IReadOnlyCollection<HunterLocationDto>>> MassGetHuntersLocationsAsync(HuntersLocationsMassGetRequest request);

}