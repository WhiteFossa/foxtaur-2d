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
    Task<IReadOnlyCollection<Distance>> GetDistancesWithoutIncludeAsync();

    /// <summary>
    /// Get distance by ID
    /// </summary>
    Task<Distance> GetDistanceByIdAsync(Guid distanceId);

    /// <summary>
    /// Mass get hunters locations (without including data on hunters, foxes and so on)
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
    Task<IReadOnlyCollection<Hunter>> MassGetHuntersAsync(HuntersMassGetRequest request, DateTime locationsHistoriesFromTime, DateTime locationsHistoriesToTime);
    
    /// <summary>
    /// Mass get locations
    /// </summary>
    Task<IReadOnlyCollection<Location>> MassGetLocationsAsync(LocationsMassGetRequest request);

    /// <summary>
    /// Mass get hunters by distance ID async (locations histories is NOT included)
    /// </summary>
    Task<IReadOnlyCollection<Hunter>> MassGetHuntersByDistanceIdWithoutLocationsHistoriesAsync(Guid distanceId);
}