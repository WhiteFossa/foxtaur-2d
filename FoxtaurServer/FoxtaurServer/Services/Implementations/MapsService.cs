using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;
using LibAuxiliary.Helpers;

namespace FoxtaurServer.Services.Implementations;

public class MapsService : IMapsService
{
    private List<MapDto> _maps = new List<MapDto>();

    public MapsService()
    {
        _maps.Add(new MapDto(new Guid("2754AEB3-9E20-4017-8858-D4E5982D3802"),
            "Давыдово",
            54.807812457.ToRadians(),
            54.757759918.ToRadians(),
            39.879142801.ToRadians(),
            39.823302090.ToRadians(),
            @"https://static.foxtaur.me/Maps/Davydovo/Davydovo.tif.zst"));
        
        _maps.Add(new MapDto(new Guid("2947B1E8-E54F-4C47-80E3-1A1E8AC045F7"),
            "Gorica",
            42.454572697.ToRadians(),
            42.440712652.ToRadians(),
            19.281242689.ToRadians(),
            19.262488444.ToRadians(),
            @"https://static.foxtaur.me/Maps/Gorica/Gorica.tif.zst"));
    }

    public async Task<IReadOnlyCollection<MapDto>> MassGetMapsAsync(IReadOnlyCollection<Guid> mapsIds)
    {
        _ = mapsIds ?? throw new ArgumentNullException(nameof(mapsIds));

        return _maps
            .Where(m => mapsIds.Contains(m.Id))
            .ToList();
    }
}