using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;
using LibAuxiliary.Helpers;

namespace FoxtaurServer.Services.Implementations;

public class MapsService : IMapsService
{
    private readonly IMapsDao _mapsDao;
    private readonly IMapsMapper _mapsMapper;

    public MapsService(IMapsDao mapsDao,
        IMapsMapper mapsMapper)
    {
        _mapsDao = mapsDao;
        _mapsMapper = mapsMapper;
    }

    public async Task<IReadOnlyCollection<MapDto>> MassGetMapsAsync(IReadOnlyCollection<Guid> mapsIds)
    {
        _ = mapsIds ?? throw new ArgumentNullException(nameof(mapsIds));

        return _mapsMapper.Map(await _mapsDao.GetMapsAsync(mapsIds));
    }

    public async Task<IReadOnlyCollection<MapDto>> GetAllMapsAsync()
    {
        return _mapsMapper.Map(await _mapsDao.GetAllMapsAsync());
    }
}