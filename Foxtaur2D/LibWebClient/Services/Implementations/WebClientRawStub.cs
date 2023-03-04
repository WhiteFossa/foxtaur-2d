using LibGeo.Implementations.Helpers;
using LibWebClient.Enums;
using LibWebClient.Models.DTOs;
using LibWebClient.Services.Abstract;

namespace LibWebClient.Services.Implementations;

public class WebClientRawStub : IWebClientRaw
{
    public async Task<TeamDto> GetTeamByIdAsync(Guid id)
    {
        switch (id.ToString().ToUpperInvariant())
        {
            case "AE9EE155-BCDC-44C3-B83F-A4837A3EC443":
                return new TeamDto(new Guid("AE9EE155-BCDC-44C3-B83F-A4837A3EC443"), "Foxtaurs");
            
            case "4E44C3DE-4B3A-472B-8289-2072A9F7B49C":
                return new TeamDto(new Guid("4E44C3DE-4B3A-472B-8289-2072A9F7B49C"), "Fox yiffers");
            
            default:
                throw new ArgumentException("Team not found!");
        }
    }

    public async Task<HunterDto> GetHunterByIdAsync(Guid id)
    {
        switch (id.ToString().ToUpperInvariant())
        {
            case "E7B81F14-5B4E-446A-9892-36B60AF6511E":
                return new HunterDto(new Guid("E7B81F14-5B4E-446A-9892-36B60AF6511E"), "Garrek", true, new Guid("AE9EE155-BCDC-44C3-B83F-A4837A3EC443"), 54.777324.ToRadians(),39.849310.ToRadians());
            
            case "42FA82C3-75B7-4837-A37A-636C173DA1AB":
                return new HunterDto(new Guid("42FA82C3-75B7-4837-A37A-636C173DA1AB"), "Goldfur", true, new Guid("4E44C3DE-4B3A-472B-8289-2072A9F7B49C"), 54.8006538.ToRadians(),39.8636070.ToRadians());
            
            case "7A598C33-9682-4DC4-95A6-656164D5D7AF":
                return new HunterDto(new Guid("7A598C33-9682-4DC4-95A6-656164D5D7AF"), "Fossa", true, new Guid("4E44C3DE-4B3A-472B-8289-2072A9F7B49C"), 42.4492759.ToRadians(),19.2731099.ToRadians());
            
            case "D2EC8AAD-B173-4E2D-A0E0-41762FE196E6":
                return new HunterDto(new Guid("D2EC8AAD-B173-4E2D-A0E0-41762FE196E6"), "Felekar", true, new Guid("AE9EE155-BCDC-44C3-B83F-A4837A3EC443"), 42.4515011.ToRadians(),19.2778191.ToRadians());
            
            default:
                throw new ArgumentException("Hunter not found");
        }
    }

    public async Task<FoxDto> GetFoxByIdAsync(Guid id)
    {
        switch (id.ToString().ToUpperInvariant())
        {
            case "FC7BB34B-F9F0-4E7A-98D1-7699CC1B4423":
                return new FoxDto(new Guid("FC7BB34B-F9F0-4E7A-98D1-7699CC1B4423"), "Emerlina", 145000000, "MOE");
            
            case "830EFB6A-0064-48AC-8BF8-70502C3A619D":
                return new FoxDto(new Guid("830EFB6A-0064-48AC-8BF8-70502C3A619D"), "Fler", 145500000, "MOI");
            
            case "B262C3A7-7D79-41C7-BE02-CAA3BB3B957B":
                return new FoxDto(new Guid("B262C3A7-7D79-41C7-BE02-CAA3BB3B957B"), "Lima", 144000000, "MOS");
            
            case "0E25C2A8-3BF3-485C-BB24-81F65BBF3EF6":
                return new FoxDto(new Guid("0E25C2A8-3BF3-485C-BB24-81F65BBF3EF6"), "Rita", 146000000, "MOH");
            
            case "F46CFCA4-937D-45A4-8302-2D2DA8E9F1AA":
                return new FoxDto(new Guid("F46CFCA4-937D-45A4-8302-2D2DA8E9F1AA"), "Krita", 146000000, "MO5");
            
            case "545A8D1C-301F-49B9-AEA6-5CFD4C8B5D9B":
                return new FoxDto(new Guid("545A8D1C-301F-49B9-AEA6-5CFD4C8B5D9B"), "Malena", 144500000, "MO");
            
            default:
                throw new ArgumentException("Fox not found");
        }
    }

    public async Task<LocationDto> GetLocationByIdAsync(Guid id)
    {
        switch (id.ToString().ToUpperInvariant())
        {
            case "6550C9C5-6945-40F1-BDC6-17898C116A32":
                // Davydovo - Start
                return new LocationDto(new Guid("6550C9C5-6945-40F1-BDC6-17898C116A32"), "Start", LocationType.Start, 54.7717312.ToRadians(), 39.8320896.ToRadians(), null);

            case "FEAA7806-7FFC-4CD8-A584-6B41B17A0E77":
                // Davydovo - Emerlina
                return new LocationDto(new Guid("FEAA7806-7FFC-4CD8-A584-6B41B17A0E77"), "Fox location", LocationType.Fox, 54.7684903.ToRadians(), 39.8525598.ToRadians(), new Guid("FC7BB34B-F9F0-4E7A-98D1-7699CC1B4423"));
            
            case "F56B2833-973A-45E2-803C-C7AB6C7752D8":
                // Davydovo - Fler
                return new LocationDto(new Guid("F56B2833-973A-45E2-803C-C7AB6C7752D8"), "Fox location", LocationType.Fox, 54.76413472.ToRadians(), 39.84555885.ToRadians(), new Guid("830EFB6A-0064-48AC-8BF8-70502C3A619D"));

            case "AA4A5E4D-5198-4198-97EB-520974785F3F":
                // Davydovo - Lima
                return new LocationDto(new Guid("AA4A5E4D-5198-4198-97EB-520974785F3F"), "Fox location", LocationType.Fox, 54.7809681.ToRadians(), 39.8478892.ToRadians(), new Guid("B262C3A7-7D79-41C7-BE02-CAA3BB3B957B"));
            
            case "1B28D943-71CC-4971-8449-2460B906EC4B":
                // Davydovo - Rita
                return new LocationDto(new Guid("1B28D943-71CC-4971-8449-2460B906EC4B"), "Fox location", LocationType.Fox, 54.79140241.ToRadians(), 39.84157976.ToRadians(), new Guid("0E25C2A8-3BF3-485C-BB24-81F65BBF3EF6"));
            
            case "6A6E5E2C-746F-4F6F-B0D0-6C71EEFA1DFF":
                // Davydovo - Krita
                return new LocationDto(new Guid("6A6E5E2C-746F-4F6F-B0D0-6C71EEFA1DFF"), "Fox location", LocationType.Fox, 54.79982684.ToRadians(), 39.85969226.ToRadians(), new Guid("F46CFCA4-937D-45A4-8302-2D2DA8E9F1AA"));
            
            case "B2E3E116-723B-4858-85BB-A6BD3BFF252B":
                // Davydovo - Malena
                return new LocationDto(new Guid("B2E3E116-723B-4858-85BB-A6BD3BFF252B"), "Fox location", LocationType.Fox, 54.7920666.ToRadians(), 39.8663579.ToRadians(), new Guid("545A8D1C-301F-49B9-AEA6-5CFD4C8B5D9B"));
            
            case "E89CD9BE-B5FB-4D35-A321-C3C575AEDE63":
                // Davydovo - Finish corridor entrance
                return new LocationDto(new Guid("E89CD9BE-B5FB-4D35-A321-C3C575AEDE63"), "Finish corridor entrance", LocationType.FinishCorridorEntrance, 54.7919942.ToRadians(), 39.8667012.ToRadians(), null);
            
            case "53ECF004-F388-4623-AABC-486BE60B6AC8":
                // Davydovo - Finish
                return new LocationDto(new Guid("53ECF004-F388-4623-AABC-486BE60B6AC8"), "Finish", LocationType.Finish, 54.79184839.ToRadians(), 39.86736020.ToRadians(), null);

            case "D2ADFE4A-38D2-472F-A79C-6D3A6A257B6C":
                // Gorica - Start
                return new LocationDto(new Guid("D2ADFE4A-38D2-472F-A79C-6D3A6A257B6C"), "Start", LocationType.Start, 42.4499615.ToRadians(), 19.2651843.ToRadians(), null);
            
            case "9D448CD1-ADED-43C5-9513-53386548BFCB":
                // Gorica - Emerlina
                return new LocationDto(new Guid("9D448CD1-ADED-43C5-9513-53386548BFCB"), "Fox location", LocationType.Fox, 42.4535335.ToRadians(), 19.2658151.ToRadians(), new Guid("FC7BB34B-F9F0-4E7A-98D1-7699CC1B4423"));
            
            case "AB533EAA-1E35-4252-AC22-DD8674C8452F":
                // Gorica - Fler
                return new LocationDto(new Guid("AB533EAA-1E35-4252-AC22-DD8674C8452F"), "Fox location", LocationType.Fox, 42.4507050.ToRadians(), 19.2702974.ToRadians(), new Guid("830EFB6A-0064-48AC-8BF8-70502C3A619D"));

            case "69A2AD21-FB01-497F-852E-B7EFC754226B":
                // Gorica - Lima
                return new LocationDto(new Guid("69A2AD21-FB01-497F-852E-B7EFC754226B"), "Fox location", LocationType.Fox, 42.4528891.ToRadians(), 19.2789995.ToRadians(), new Guid("B262C3A7-7D79-41C7-BE02-CAA3BB3B957B"));
            
            case "2C0AAF06-747F-4DB4-A544-D042299F81DD":
                // Gorica - Rita
                return new LocationDto(new Guid("2C0AAF06-747F-4DB4-A544-D042299F81DD"), "Fox location", LocationType.Fox, 42.4484981.ToRadians(), 19.2744431.ToRadians(), new Guid("0E25C2A8-3BF3-485C-BB24-81F65BBF3EF6"));
            
            case "4A4B9605-91DA-4DB7-84CC-B1BC932949FB":
                // Gorica - Krita
                return new LocationDto(new Guid("4A4B9605-91DA-4DB7-84CC-B1BC932949FB"), "Fox location", LocationType.Fox, 42.4445633.ToRadians(), 19.2692424.ToRadians(), new Guid("F46CFCA4-937D-45A4-8302-2D2DA8E9F1AA"));
            
            case "94FFACBF-9BFC-48AA-B449-DE360DCDC6B9":
                // Gorica - Malena
                return new LocationDto(new Guid("94FFACBF-9BFC-48AA-B449-DE360DCDC6B9"), "Fox location", LocationType.Fox, 42.4493934.ToRadians(), 19.2672465.ToRadians(), new Guid("545A8D1C-301F-49B9-AEA6-5CFD4C8B5D9B"));
            
            case "3EF50875-524C-4B3C-9EEA-4E339023B777":
                // Gorica - Finish corridor entrance
                return new LocationDto(new Guid("3EF50875-524C-4B3C-9EEA-4E339023B777"), "Finish corridor entrance", LocationType.FinishCorridorEntrance, 42.4494860.ToRadians(), 19.2669100.ToRadians(), null);
            
            case "003062D4-1347-48DA-9193-F90652B09A7E":
                // Gorica - Finish
                return new LocationDto(new Guid("003062D4-1347-48DA-9193-F90652B09A7E"), "Finish", LocationType.Finish, 42.4496250.ToRadians(), 19.2662307.ToRadians(), null);
            
            default:
                throw new ArgumentException("Location not found");
        }
    }

    public async Task<MapDto> GetMapByIdAsync(Guid id)
    {
        switch (id.ToString().ToUpperInvariant())
        {
            case "2754AEB3-9E20-4017-8858-D4E5982D3802":
                return new MapDto(new Guid("2754AEB3-9E20-4017-8858-D4E5982D3802"),
                    "Давыдово",
                    54.807812457.ToRadians(),
                    54.757759918.ToRadians(),
                    39.879142801.ToRadians(),
                    39.823302090.ToRadians(),
                    @"Maps/Davydovo/Davydovo.tif.zst");

            case "2947B1E8-E54F-4C47-80E3-1A1E8AC045F7":
                return new MapDto(new Guid("2947B1E8-E54F-4C47-80E3-1A1E8AC045F7"),
                    "Gorica",
                    42.454572697.ToRadians(),
                    42.440712652.ToRadians(),
                    19.281242689.ToRadians(),
                    19.262488444.ToRadians(),
                    @"Maps/Gorica/Gorica.tif.zst");

            default:
                throw new ArgumentException("Wrong ID");
        }
    }

    public async Task<DistanceDto> GetDistanceByIdAsync(Guid id)
    {
        switch (id.ToString().ToUpperInvariant())
        {
            case "89E7EC2D-C7E3-42B6-BBB8-C340E681FCBE":
                return new DistanceDto(
                    new Guid("89E7EC2D-C7E3-42B6-BBB8-C340E681FCBE"),
                    "Давыдово",
                    new Guid("2754AEB3-9E20-4017-8858-D4E5982D3802"),
                    true,
                    new Guid("6550C9C5-6945-40F1-BDC6-17898C116A32"),
                    new Guid("E89CD9BE-B5FB-4D35-A321-C3C575AEDE63"),
                    new Guid("53ECF004-F388-4623-AABC-486BE60B6AC8"),
                    new List<Guid>
                    {
                        new Guid("FEAA7806-7FFC-4CD8-A584-6B41B17A0E77"),
                        new Guid("F56B2833-973A-45E2-803C-C7AB6C7752D8"),
                        new Guid("AA4A5E4D-5198-4198-97EB-520974785F3F"),
                        new Guid("1B28D943-71CC-4971-8449-2460B906EC4B"),
                        new Guid("6A6E5E2C-746F-4F6F-B0D0-6C71EEFA1DFF"),
                        new Guid("B2E3E116-723B-4858-85BB-A6BD3BFF252B")
                    },
                    new List<Guid>
                    {
                        new Guid("F56B2833-973A-45E2-803C-C7AB6C7752D8"),
                        new Guid("FEAA7806-7FFC-4CD8-A584-6B41B17A0E77"),
                        new Guid("AA4A5E4D-5198-4198-97EB-520974785F3F"),
                        new Guid("1B28D943-71CC-4971-8449-2460B906EC4B"),
                        new Guid("6A6E5E2C-746F-4F6F-B0D0-6C71EEFA1DFF"),
                        new Guid("B2E3E116-723B-4858-85BB-A6BD3BFF252B")
                    },
                    new List<Guid> { new Guid("E7B81F14-5B4E-446A-9892-36B60AF6511E"), new Guid("42FA82C3-75B7-4837-A37A-636C173DA1AB") }
                );

            case "A59E6C8F-4C5E-47B4-9EF2-8D1B25CD569C":
                return new DistanceDto(
                    new Guid("A59E6C8F-4C5E-47B4-9EF2-8D1B25CD569C"),
                    "Gorica",
                    new Guid("2947B1E8-E54F-4C47-80E3-1A1E8AC045F7"),
                    true,
                    new Guid("D2ADFE4A-38D2-472F-A79C-6D3A6A257B6C"),
                    new Guid("3EF50875-524C-4B3C-9EEA-4E339023B777"),
                    new Guid("003062D4-1347-48DA-9193-F90652B09A7E"),
                    new List<Guid>
                    {
                        new Guid("9D448CD1-ADED-43C5-9513-53386548BFCB"),
                        new Guid("AB533EAA-1E35-4252-AC22-DD8674C8452F"),
                        new Guid("69A2AD21-FB01-497F-852E-B7EFC754226B"),
                        new Guid("2C0AAF06-747F-4DB4-A544-D042299F81DD"),
                        new Guid("4A4B9605-91DA-4DB7-84CC-B1BC932949FB"),
                        new Guid("94FFACBF-9BFC-48AA-B449-DE360DCDC6B9")
                    },
                    new List<Guid>
                    {
                        new Guid("9D448CD1-ADED-43C5-9513-53386548BFCB"),
                        new Guid("AB533EAA-1E35-4252-AC22-DD8674C8452F"),
                        new Guid("69A2AD21-FB01-497F-852E-B7EFC754226B"),
                        new Guid("2C0AAF06-747F-4DB4-A544-D042299F81DD"),
                        new Guid("4A4B9605-91DA-4DB7-84CC-B1BC932949FB"),
                        new Guid("94FFACBF-9BFC-48AA-B449-DE360DCDC6B9")
                    },
                    new List<Guid> { new Guid("7A598C33-9682-4DC4-95A6-656164D5D7AF"), new Guid("D2EC8AAD-B173-4E2D-A0E0-41762FE196E6") }
                );

            default:
                throw new ArgumentException("Wrong ID");
        }
    }

    public async Task<IReadOnlyCollection<DistanceDto>> ListDistancesAsync()
    {
        return new List<DistanceDto>()
        {
            new DistanceDto(
                new Guid("89E7EC2D-C7E3-42B6-BBB8-C340E681FCBE"),
                "Давыдово",
                new Guid("2754AEB3-9E20-4017-8858-D4E5982D3802"),
                true,
                new Guid("6550C9C5-6945-40F1-BDC6-17898C116A32"),
                new Guid("E89CD9BE-B5FB-4D35-A321-C3C575AEDE63"),
                new Guid("53ECF004-F388-4623-AABC-486BE60B6AC8"),
                new List<Guid>
                {
                    new Guid("FEAA7806-7FFC-4CD8-A584-6B41B17A0E77"),
                    new Guid("F56B2833-973A-45E2-803C-C7AB6C7752D8"),
                    new Guid("AA4A5E4D-5198-4198-97EB-520974785F3F"),
                    new Guid("1B28D943-71CC-4971-8449-2460B906EC4B"),
                    new Guid("6A6E5E2C-746F-4F6F-B0D0-6C71EEFA1DFF"),
                    new Guid("B2E3E116-723B-4858-85BB-A6BD3BFF252B")
                },
                new List<Guid>
                {
                    new Guid("F56B2833-973A-45E2-803C-C7AB6C7752D8"),
                    new Guid("FEAA7806-7FFC-4CD8-A584-6B41B17A0E77"),
                    new Guid("AA4A5E4D-5198-4198-97EB-520974785F3F"),
                    new Guid("1B28D943-71CC-4971-8449-2460B906EC4B"),
                    new Guid("6A6E5E2C-746F-4F6F-B0D0-6C71EEFA1DFF"),
                    new Guid("B2E3E116-723B-4858-85BB-A6BD3BFF252B")
                },
                new List<Guid> { new Guid("E7B81F14-5B4E-446A-9892-36B60AF6511E"), new Guid("42FA82C3-75B7-4837-A37A-636C173DA1AB") }
            ),
            
            new DistanceDto(
                new Guid("A59E6C8F-4C5E-47B4-9EF2-8D1B25CD569C"),
                "Gorica",
                new Guid("2947B1E8-E54F-4C47-80E3-1A1E8AC045F7"),
                true,
                new Guid("D2ADFE4A-38D2-472F-A79C-6D3A6A257B6C"),
                new Guid("3EF50875-524C-4B3C-9EEA-4E339023B777"),
                new Guid("003062D4-1347-48DA-9193-F90652B09A7E"),
                new List<Guid>
                {
                    new Guid("9D448CD1-ADED-43C5-9513-53386548BFCB"),
                    new Guid("AB533EAA-1E35-4252-AC22-DD8674C8452F"),
                    new Guid("69A2AD21-FB01-497F-852E-B7EFC754226B"),
                    new Guid("2C0AAF06-747F-4DB4-A544-D042299F81DD"),
                    new Guid("4A4B9605-91DA-4DB7-84CC-B1BC932949FB"),
                    new Guid("94FFACBF-9BFC-48AA-B449-DE360DCDC6B9")
                },
                new List<Guid>
                {
                    new Guid("9D448CD1-ADED-43C5-9513-53386548BFCB"),
                    new Guid("AB533EAA-1E35-4252-AC22-DD8674C8452F"),
                    new Guid("69A2AD21-FB01-497F-852E-B7EFC754226B"),
                    new Guid("2C0AAF06-747F-4DB4-A544-D042299F81DD"),
                    new Guid("4A4B9605-91DA-4DB7-84CC-B1BC932949FB"),
                    new Guid("94FFACBF-9BFC-48AA-B449-DE360DCDC6B9")
                },
                new List<Guid> { new Guid("7A598C33-9682-4DC4-95A6-656164D5D7AF"), new Guid("D2EC8AAD-B173-4E2D-A0E0-41762FE196E6") }
            )
        };
    }
}