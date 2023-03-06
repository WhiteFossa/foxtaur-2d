using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;
using LibAuxiliary.Helpers;

namespace FoxtaurServer.Services.Implementations;

public class HuntersService : IHuntersService
{
    private List<HunterDto> _hunters = new List<HunterDto>();

    public HuntersService()
    {
        _hunters.Add(new HunterDto(new Guid("E7B81F14-5B4E-446A-9892-36B60AF6511E"),
            "Garrek",
            true,
            new Guid("AE9EE155-BCDC-44C3-B83F-A4837A3EC443"),
            new HunterLocationDto(new Guid("00E0F9FC-F24F-47D4-9D6B-987AEF912261"), DateTime.UtcNow, 54.777324.ToRadians(),39.849310.ToRadians(), 130.0)));
        
        _hunters.Add(new HunterDto(new Guid("42FA82C3-75B7-4837-A37A-636C173DA1AB"),
            "Goldfur",
            true,
            new Guid("4E44C3DE-4B3A-472B-8289-2072A9F7B49C"),
            new HunterLocationDto(new Guid("BB60D762-FE1E-47F7-A2CE-BB921A61870D"), DateTime.UtcNow, 54.8006538.ToRadians(),39.8636070.ToRadians(), 135.1)));
        
        _hunters.Add(new HunterDto(new Guid("7A598C33-9682-4DC4-95A6-656164D5D7AF"),
            "Fossa",
            true,
            new Guid("4E44C3DE-4B3A-472B-8289-2072A9F7B49C"),
            new HunterLocationDto(new Guid("F098DEDD-5C96-4713-8454-083260E2E5ED"), DateTime.UtcNow, 42.4492759.ToRadians(),19.2731099.ToRadians(), 73.2)));
        
        _hunters.Add(new HunterDto(new Guid("D2EC8AAD-B173-4E2D-A0E0-41762FE196E6"),
            "Felekar",
            true,
            new Guid("AE9EE155-BCDC-44C3-B83F-A4837A3EC443"),
            new HunterLocationDto(new Guid("ACFD1A28-276F-4B8C-8F22-20C1B8ECBEAD"), DateTime.UtcNow, 42.4515011.ToRadians(),19.2778191.ToRadians(), 75.8)));
    }
    
    public async Task<HunterDto> GetHunterByIdAsync(Guid id)
    {
        return _hunters
            .SingleOrDefault(h => h.Id == id);
    }
}