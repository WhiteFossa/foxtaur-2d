using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;
using LibAuxiliary.Helpers;

namespace FoxtaurServer.Services.Implementations;

public class HuntersLocationsService : IHuntersLocationsService
{
    private Dictionary<Guid, List<HunterLocationDto>> _locationsByHunters = new Dictionary<Guid, List<HunterLocationDto>>();

    public HuntersLocationsService()
    {
        // Garrek
        _locationsByHunters.Add(new Guid("E7B81F14-5B4E-446A-9892-36B60AF6511E"),
            new List<HunterLocationDto>()
            {
                new HunterLocationDto(new Guid("00E0F9FC-F24F-47D4-9D6B-987AEF912261"), DateTime.UtcNow, 54.777324.ToRadians(),39.849310.ToRadians(), 130.0), // Newest
                new HunterLocationDto(new Guid("9369F744-C49C-462A-9E16-6D251312D6BB"), new DateTime(2023, 03, 06, 08, 10, 01), 54.775352.ToRadians(), 39.850687.ToRadians(), 129.0),
                new HunterLocationDto(new Guid("65DA1BC5-10AD-497C-AC1B-342E8ED5F714"), new DateTime(2023, 03, 06, 08, 09, 01), 54.772510.ToRadians(), 39.852705.ToRadians(), 128.0),
                new HunterLocationDto(new Guid("5983399C-43B4-40FA-8591-B3B023B9E19A"), new DateTime(2023, 03, 06, 08, 08, 01), 54.768484.ToRadians(), 39.852592.ToRadians(), 127.0),
                new HunterLocationDto(new Guid("8E8F206E-FFEC-4EBE-942E-3FEB9A7FEAD6"), new DateTime(2023, 03, 06, 08, 07, 01), 54.767310.ToRadians(), 39.851500.ToRadians(), 126.0),
                new HunterLocationDto(new Guid("93887FBB-3233-4322-B2BA-712B42034A23"), new DateTime(2023, 03, 06, 08, 06, 01), 54.767310.ToRadians(), 39.846867.ToRadians(), 125.0),
                new HunterLocationDto(new Guid("9914CC78-A9EC-4C98-BE4C-916EB1962CCE"), new DateTime(2023, 03, 06, 08, 05, 01), 54.764139.ToRadians(), 39.845569.ToRadians(), 124.0),
                new HunterLocationDto(new Guid("78EC95B2-527D-4203-B037-061F08EB4748"), new DateTime(2023, 03, 06, 08, 04, 01), 54.763778.ToRadians(), 39.843026.ToRadians(), 123.0),
                new HunterLocationDto(new Guid("DECC1E13-E137-40D0-BB90-ED6C9E3C628F"), new DateTime(2023, 03, 06, 08, 03, 01), 54.766126.ToRadians(), 39.840709.ToRadians(), 122.0),
                new HunterLocationDto(new Guid("7FE04654-0B34-4BC7-AC52-D460B7045A05"), new DateTime(2023, 03, 06, 08, 02, 01), 54.769184.ToRadians(), 39.836498.ToRadians(), 121.0),
                new HunterLocationDto(new Guid("1A328857-CD5A-4D96-84F6-C0F90B364794"), new DateTime(2023, 03, 06, 08, 01, 01), 54.7717312.ToRadians(), 39.8320896.ToRadians(), 120.0) // Start
            });
        
        // Goldfur
        _locationsByHunters.Add(new Guid("42FA82C3-75B7-4837-A37A-636C173DA1AB"),
            new List<HunterLocationDto>()
            {
                new HunterLocationDto(new Guid("BB60D762-FE1E-47F7-A2CE-BB921A61870D"), DateTime.UtcNow, 54.8006538.ToRadians(),39.8636070.ToRadians(), 135.1)
            });
        
        // Fossa
        _locationsByHunters.Add(new Guid("7A598C33-9682-4DC4-95A6-656164D5D7AF"),
            new List<HunterLocationDto>()
            {
                new HunterLocationDto(new Guid("F098DEDD-5C96-4713-8454-083260E2E5ED"), DateTime.UtcNow, 42.4492759.ToRadians(),19.2731099.ToRadians(), 73.2)
            });
        
        // Felekar
        _locationsByHunters.Add(new Guid("D2EC8AAD-B173-4E2D-A0E0-41762FE196E6"),
            new List<HunterLocationDto>()
            {
                new HunterLocationDto(new Guid("ACFD1A28-276F-4B8C-8F22-20C1B8ECBEAD"), DateTime.UtcNow, 42.4515011.ToRadians(),19.2778191.ToRadians(), 75.8)
            });
    }
    
    public async Task<HunterLocationDto> GetHunterLocationByIdAsync(Guid id)
    {
        return _locationsByHunters
            .SelectMany(lbh => lbh.Value)
            .SingleOrDefault(hl => hl.Id == id);
    }

    public async Task<HunterLocationDto> GetLastHunterLocationByHunterId(Guid id)
    {
        if (!_locationsByHunters.ContainsKey(id))
        {
            return null;
        }

        return _locationsByHunters[id]
            .OrderBy(hl => hl.Timestamp)
            .Last();
    }

    public async Task<IReadOnlyCollection<HunterLocationDto>> GetHunterLocationsHistoryByHunterId(Guid id)
    {
        if (!_locationsByHunters.ContainsKey(id))
        {
            return null;
        }

        return _locationsByHunters[id]
            .OrderBy(hl => hl.Timestamp)
            .ToList();
    }
}