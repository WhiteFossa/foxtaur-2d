using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Services.Abstract;

/// <summary>
/// Service to work with foxes
/// </summary>
public interface IFoxesService
{
    /// <summary>
    /// Get fox by ID
    /// Will return null if fox with given ID wasn't found
    /// </summary>
    Task<FoxDto> GetFoxByIdAsync(Guid id);

    /// <summary>
    /// Mass get foxes by their IDs
    /// </summary>
    Task<IReadOnlyCollection<FoxDto>> MassGetFoxesAsync(IReadOnlyCollection<Guid> foxesIds);
}