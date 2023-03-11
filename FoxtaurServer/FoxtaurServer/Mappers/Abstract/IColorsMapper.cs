using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Mappers.Abstract;

/// <summary>
/// Colors mapper
/// </summary>
public interface IColorsMapper
{
    ColorDto Map(byte r, byte g, byte b, byte a);
}