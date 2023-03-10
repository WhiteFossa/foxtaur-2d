using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Services.Abstract;

/// <summary>
/// Service to work with hunters
/// </summary>
public interface IHuntersService
{
    /// <summary>
    /// Mass get hunters by their IDs
    /// </summary>
    Task<IReadOnlyCollection<HunterDto>> MassGetHuntersAsync(IReadOnlyCollection<Guid> huntersIds);
}