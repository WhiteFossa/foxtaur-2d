using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Services.Abstract;

/// <summary>
/// Service to work with foxes
/// </summary>
public interface IFoxesService
{
    /// <summary>
    /// Mass get foxes by their IDs
    /// </summary>
    Task<IReadOnlyCollection<FoxDto>> MassGetFoxesAsync(IReadOnlyCollection<Guid> foxesIds);
    
    /// <summary>
    /// Get all foxes
    /// </summary>
    Task<IReadOnlyCollection<FoxDto>> GetAllFoxesAsync();
    
    /// <summary>
    /// Create new fox. Will return null in case of failure
    /// </summary>
    Task<FoxDto> CreateNewFoxAsync(FoxDto fox);
}