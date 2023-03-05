using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;

namespace FoxtaurServer.Services.Implementations;

public class FoxesService : IFoxesService
{
    private List<FoxDto> _foxes = new List<FoxDto>();

    public FoxesService()
    {
        _foxes.Add(new FoxDto(new Guid("FC7BB34B-F9F0-4E7A-98D1-7699CC1B4423"), "Emerlina", 145000000, "MOE"));

        _foxes.Add(new FoxDto(new Guid("830EFB6A-0064-48AC-8BF8-70502C3A619D"), "Fler", 145500000, "MOI"));

        _foxes.Add(new FoxDto(new Guid("B262C3A7-7D79-41C7-BE02-CAA3BB3B957B"), "Lima", 144000000, "MOS"));

        _foxes.Add(new FoxDto(new Guid("0E25C2A8-3BF3-485C-BB24-81F65BBF3EF6"), "Rita", 146000000, "MOH"));
        
        _foxes.Add(new FoxDto(new Guid("F46CFCA4-937D-45A4-8302-2D2DA8E9F1AA"), "Krita", 146000000, "MO5"));
        
        _foxes.Add(new FoxDto(new Guid("545A8D1C-301F-49B9-AEA6-5CFD4C8B5D9B"), "Malena", 144500000, "MO"));
    }

    public async Task<FoxDto> GetFoxByIdAsync(Guid id)
    {
        return _foxes
            .SingleOrDefault(f => f.Id == id);
    }
}