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
}