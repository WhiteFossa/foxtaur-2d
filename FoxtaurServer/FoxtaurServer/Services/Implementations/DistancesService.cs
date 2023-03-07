using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;

namespace FoxtaurServer.Services.Implementations;

public class DistancesService : IDistancesService
{
    private List<DistanceDto> _distances = new List<DistanceDto>();

    public DistancesService()
    {
        _distances.Add(new DistanceDto(
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
            new List<Guid> { new Guid("E7B81F14-5B4E-446A-9892-36B60AF6511E"), new Guid("42FA82C3-75B7-4837-A37A-636C173DA1AB") },
            new DateTime(2023, 03, 06, 08, 00, 00)
        ));
        
        _distances.Add(new DistanceDto(
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
            new List<Guid> { new Guid("7A598C33-9682-4DC4-95A6-656164D5D7AF"), new Guid("D2EC8AAD-B173-4E2D-A0E0-41762FE196E6") },
            new DateTime(2023, 03, 06, 08, 00, 00)
        ));
    }
    
    public async Task<IReadOnlyCollection<DistanceDto>> ListDistancesAsync()
    {
        return _distances.AsReadOnly();
    }

    public async Task<DistanceDto> GetDistanceById(Guid id)
    {
        return _distances
            .SingleOrDefault(d => d.Id == id);
    }
}