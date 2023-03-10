using LibWebClient.Models;
using LibWebClient.Models.Requests;

namespace LibWebClient.Services.Abstract;

/// <summary>
/// High-level web client
/// </summary>
public interface IWebClient
{
    /// <summary>
    /// Get list of all distances (without including data on hunters, foxes and so on)
    /// </summary>
    /// <returns></returns>
    Task<IReadOnlyCollection<Distance>> GetDistancesWithoutIncludeAsync();

    /// <summary>
    /// Get distance by ID
    /// </summary>
    Task<Distance> GetDistanceByIdAsync(Guid distanceId);
    
    /// <summary>
    /// Mass get hunters locations
    /// </summary>
    Task<Dictionary<Guid, IReadOnlyCollection<HunterLocation>>> MassGetHuntersLocationsAsync(HuntersLocationsMassGetRequest request);

    /// <summary>
    /// Mass get foxes
    /// </summary>
    Task<IReadOnlyCollection<Fox>> MassGetFoxesAsync(FoxesMassGetRequest request);

    /// <summary>
    /// Mass get teams
    /// </summary>
    Task<IReadOnlyCollection<Team>> MassGetTeamsAsync(TeamsMassGetRequest request);
    
    /// <summary>
    /// Mass get maps
    /// </summary>
    Task<IReadOnlyCollection<Map>> MassGetMapsAsync(MapsMassGetRequest request);
    
    /// <summary>
    /// Mass get hunters
    /// </summary>
    Task<IReadOnlyCollection<Hunter>> MassGetHuntersAsync(HuntersMassGetRequest request, DateTime locationsHistoriesFromTime);
}