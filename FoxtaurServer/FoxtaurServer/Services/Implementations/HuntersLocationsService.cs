using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;
using LibAuxiliary.Helpers;

namespace FoxtaurServer.Services.Implementations;

public class HuntersLocationsService : IHuntersLocationsService
{
    private List<HunterLocationDto> _huntersLocations = new List<HunterLocationDto>();

    public HuntersLocationsService()
    {
        _huntersLocations.Add(new HunterLocationDto(new Guid("00E0F9FC-F24F-47D4-9D6B-987AEF912261"), DateTime.UtcNow, 54.777324.ToRadians(),39.849310.ToRadians(), 130.0));
        
        _huntersLocations.Add(new HunterLocationDto(new Guid("BB60D762-FE1E-47F7-A2CE-BB921A61870D"), DateTime.UtcNow, 54.8006538.ToRadians(),39.8636070.ToRadians(), 135.1));

        _huntersLocations.Add(new HunterLocationDto(new Guid("F098DEDD-5C96-4713-8454-083260E2E5ED"), DateTime.UtcNow, 42.4492759.ToRadians(),19.2731099.ToRadians(), 73.2));

        _huntersLocations.Add(new HunterLocationDto(new Guid("ACFD1A28-276F-4B8C-8F22-20C1B8ECBEAD"), DateTime.UtcNow, 42.4515011.ToRadians(),19.2778191.ToRadians(), 75.8));
    }
    
    public async Task<HunterLocationDto> GetHunterLocationByIdAsync(Guid id)
    {
        return _huntersLocations
            .SingleOrDefault(hl => hl.Id == id);
    }
}