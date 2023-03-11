using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Mappers.Implementations;

public class ColorsMapper : IColorsMapper
{
    public ColorDto Map(byte r, byte g, byte b, byte a)
    {
        return new ColorDto(r, g, b, a);
    }
}