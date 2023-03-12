using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Services.Abstract;

/// <summary>
/// Service to work with distances
/// </summary>
public interface IDistancesService
{
    /// <summary>
    /// List all distances
    /// </summary>
    Task<IReadOnlyCollection<DistanceDto>> ListDistancesAsync();

    /// <summary>
    /// Returns distance by ID.
    /// Will return null if ID is incorrect
    /// </summary>
    Task<DistanceDto> GetDistanceById(Guid id);
    
    /// <summary>
    /// Create new distance. Can return null in case of failure
    /// </summary>
    Task<DistanceDto> CreateNewDistanceAsync(DistanceDto distance);
}