using FoxtaurServer.Models.Api;
using FoxtaurServer.Models.Api.Enums;
using FoxtaurServer.Services.Abstract;
using LibAuxiliary.Helpers;

namespace FoxtaurServer.Services.Implementations;

public class LocationsService : ILocationsService
{
    private List<LocationDto> _locations = new List<LocationDto>();

    public LocationsService()
    {
        // Davydovo - Start
        _locations.Add(new LocationDto(new Guid("6550C9C5-6945-40F1-BDC6-17898C116A32"), "Start", LocationType.Start, 54.7717312.ToRadians(), 39.8320896.ToRadians(), null));
        
        // Davydovo - Emerlina
        _locations.Add(new LocationDto(new Guid("FEAA7806-7FFC-4CD8-A584-6B41B17A0E77"), "Fox location", LocationType.Fox, 54.7684903.ToRadians(), 39.8525598.ToRadians(), new Guid("FC7BB34B-F9F0-4E7A-98D1-7699CC1B4423")));
        
        // Davydovo - Fler
        _locations.Add(new LocationDto(new Guid("F56B2833-973A-45E2-803C-C7AB6C7752D8"), "Fox location", LocationType.Fox, 54.76413472.ToRadians(), 39.84555885.ToRadians(), new Guid("830EFB6A-0064-48AC-8BF8-70502C3A619D")));
        
        // Davydovo - Lima
        _locations.Add(new LocationDto(new Guid("AA4A5E4D-5198-4198-97EB-520974785F3F"), "Fox location", LocationType.Fox, 54.7809681.ToRadians(), 39.8478892.ToRadians(), new Guid("B262C3A7-7D79-41C7-BE02-CAA3BB3B957B")));
        
        // Davydovo - Rita
        _locations.Add(new LocationDto(new Guid("1B28D943-71CC-4971-8449-2460B906EC4B"), "Fox location", LocationType.Fox, 54.79140241.ToRadians(), 39.84157976.ToRadians(), new Guid("0E25C2A8-3BF3-485C-BB24-81F65BBF3EF6")));
        
        // Davydovo - Krita
        _locations.Add(new LocationDto(new Guid("6A6E5E2C-746F-4F6F-B0D0-6C71EEFA1DFF"), "Fox location", LocationType.Fox, 54.79982684.ToRadians(), 39.85969226.ToRadians(), new Guid("F46CFCA4-937D-45A4-8302-2D2DA8E9F1AA")));
        
        // Davydovo - Malena
        _locations.Add(new LocationDto(new Guid("B2E3E116-723B-4858-85BB-A6BD3BFF252B"), "Fox location", LocationType.Fox, 54.7920666.ToRadians(), 39.8663579.ToRadians(), new Guid("545A8D1C-301F-49B9-AEA6-5CFD4C8B5D9B")));
        
        // Davydovo - Finish corridor entrance
        _locations.Add(new LocationDto(new Guid("E89CD9BE-B5FB-4D35-A321-C3C575AEDE63"), "Finish corridor entrance", LocationType.FinishCorridorEntrance, 54.7919942.ToRadians(), 39.8667012.ToRadians(), null));
        
        // Davydovo - Finish
        _locations.Add(new LocationDto(new Guid("53ECF004-F388-4623-AABC-486BE60B6AC8"), "Finish", LocationType.Finish, 54.79184839.ToRadians(), 39.86736020.ToRadians(), null));
        
        // Gorica - Start
        _locations.Add(new LocationDto(new Guid("D2ADFE4A-38D2-472F-A79C-6D3A6A257B6C"), "Start", LocationType.Start, 42.4499615.ToRadians(), 19.2651843.ToRadians(), null));
        
        // Gorica - Emerlina
        _locations.Add(new LocationDto(new Guid("9D448CD1-ADED-43C5-9513-53386548BFCB"), "Fox location", LocationType.Fox, 42.4535335.ToRadians(), 19.2658151.ToRadians(), new Guid("FC7BB34B-F9F0-4E7A-98D1-7699CC1B4423")));
        
        // Gorica - Fler
        _locations.Add(new LocationDto(new Guid("AB533EAA-1E35-4252-AC22-DD8674C8452F"), "Fox location", LocationType.Fox, 42.4507050.ToRadians(), 19.2702974.ToRadians(), new Guid("830EFB6A-0064-48AC-8BF8-70502C3A619D")));
        
        // Gorica - Lima
        _locations.Add(new LocationDto(new Guid("69A2AD21-FB01-497F-852E-B7EFC754226B"), "Fox location", LocationType.Fox, 42.4528891.ToRadians(), 19.2789995.ToRadians(), new Guid("B262C3A7-7D79-41C7-BE02-CAA3BB3B957B")));
        
        // Gorica - Rita
        _locations.Add(new LocationDto(new Guid("2C0AAF06-747F-4DB4-A544-D042299F81DD"), "Fox location", LocationType.Fox, 42.4484981.ToRadians(), 19.2744431.ToRadians(), new Guid("0E25C2A8-3BF3-485C-BB24-81F65BBF3EF6")));
        
        // Gorica - Krita
        _locations.Add(new LocationDto(new Guid("4A4B9605-91DA-4DB7-84CC-B1BC932949FB"), "Fox location", LocationType.Fox, 42.4445633.ToRadians(), 19.2692424.ToRadians(), new Guid("F46CFCA4-937D-45A4-8302-2D2DA8E9F1AA")));
        
        // Gorica - Malena
        _locations.Add(new LocationDto(new Guid("94FFACBF-9BFC-48AA-B449-DE360DCDC6B9"), "Fox location", LocationType.Fox, 42.4493934.ToRadians(), 19.2672465.ToRadians(), new Guid("545A8D1C-301F-49B9-AEA6-5CFD4C8B5D9B")));
        
        // Gorica - Finish corridor entrance
        _locations.Add(new LocationDto(new Guid("3EF50875-524C-4B3C-9EEA-4E339023B777"), "Finish corridor entrance", LocationType.FinishCorridorEntrance, 42.4494860.ToRadians(), 19.2669100.ToRadians(), null));
        
        // Gorica - Finish
        _locations.Add(new LocationDto(new Guid("003062D4-1347-48DA-9193-F90652B09A7E"), "Finish", LocationType.Finish, 42.4496250.ToRadians(), 19.2662307.ToRadians(), null));
    }
    
    public async Task<LocationDto> GetLocationByIdAsync(Guid id)
    {
        return _locations
            .SingleOrDefault(l => l.Id == id);
    }
}