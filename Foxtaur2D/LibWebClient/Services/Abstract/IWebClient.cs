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
    /// Get hunter by ID
    /// </summary>
    Task<Hunter> GetHunterByIdAsync(Guid hunterId, DateTime loadLocationsFrom);

    /// <summary>
    /// Mass get hunters locations
    /// </summary>
    Task<Dictionary<Guid, IReadOnlyCollection<HunterLocation>>> MassGetHuntersLocationsAsync(HuntersLocationsMassGetRequest request);
}