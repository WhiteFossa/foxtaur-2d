using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;
using LibAuxiliary.Helpers;

namespace FoxtaurServer.Services.Implementations;

public class HuntersService : IHuntersService
{
    private List<HunterDto> _hunters = new List<HunterDto>();

    public HuntersService()
    {
        _hunters.Add(new HunterDto(new Guid("E7B81F14-5B4E-446A-9892-36B60AF6511E"), "Garrek", true, new Guid("AE9EE155-BCDC-44C3-B83F-A4837A3EC443"), 54.777324.ToRadians(),39.849310.ToRadians()));
        
        _hunters.Add(new HunterDto(new Guid("42FA82C3-75B7-4837-A37A-636C173DA1AB"), "Goldfur", true, new Guid("4E44C3DE-4B3A-472B-8289-2072A9F7B49C"), 54.8006538.ToRadians(),39.8636070.ToRadians()));
        
        _hunters.Add(new HunterDto(new Guid("7A598C33-9682-4DC4-95A6-656164D5D7AF"), "Fossa", true, new Guid("4E44C3DE-4B3A-472B-8289-2072A9F7B49C"), 42.4492759.ToRadians(),19.2731099.ToRadians()));
        
        _hunters.Add(new HunterDto(new Guid("D2EC8AAD-B173-4E2D-A0E0-41762FE196E6"), "Felekar", true, new Guid("AE9EE155-BCDC-44C3-B83F-A4837A3EC443"), 42.4515011.ToRadians(),19.2778191.ToRadians()));
    }
    
    public async Task<HunterDto> GetHunterByIdAsync(Guid id)
    {
        return _hunters
            .SingleOrDefault(h => h.Id == id);
    }
}