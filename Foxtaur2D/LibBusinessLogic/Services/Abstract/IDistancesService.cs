using LibWebClient.Models;

namespace LibBusinessLogic.Services.Abstract;

/// <summary>
/// Service to work with distances
/// </summary>
public interface IDistancesService
{
    /// <summary>
    /// Process distance, got from server. Only processed distances are ready for use
    /// </summary>
    Distance ProcessRawDistance(Distance distance);
}