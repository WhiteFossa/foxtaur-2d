using FoxtaurServer.Dao.Models;
using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Mappers.Abstract;

/// <summary>
/// Foxes mapper
/// </summary>
public interface IFoxesMapper
{
    IReadOnlyCollection<FoxDto> Map(IEnumerable<Fox> foxes);

    FoxDto Map(Fox fox);

    Fox Map(FoxDto fox);

    IReadOnlyCollection<Fox> Map(IEnumerable<FoxDto> foxes);
}