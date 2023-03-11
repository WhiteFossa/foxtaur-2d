using FoxtaurServer.Dao.Models;
using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Mappers.Implementations;

public class FoxesMapper : IFoxesMapper
{
    public IReadOnlyCollection<FoxDto> Map(IEnumerable<Fox> foxes)
    {
        if (foxes == null)
        {
            return null;
        }

        return foxes.Select(f => Map(f)).ToList();
    }

    public FoxDto Map(Fox fox)
    {
        if (fox == null)
        {
            return null;
        }

        return new FoxDto(fox.Id, fox.Name, fox.Frequency, fox.Code);
    }

    public Fox Map(FoxDto fox)
    {
        if (fox == null)
        {
            return null;
        }

        return new Fox()
        {
            Id = fox.Id,
            Name = fox.Name,
            Frequency = fox.Frequency,
            Code = fox.Code
        };
    }

    public IReadOnlyCollection<Fox> Map(IEnumerable<FoxDto> foxes)
    {
        if (foxes == null)
        {
            return null;
        }

        return foxes.Select(f => Map(f)).ToList();
    }
}