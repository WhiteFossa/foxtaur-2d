using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;

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

    public async Task<MapDto> CreateNewMapAsync(MapDto map)
    {
        _ = map ?? throw new ArgumentNullException(nameof(map));
        
        // Do we have map with such name?
        var existingMap = await _mapsDao.GetMapByNameAsync(map.Name);
        if (existingMap != null)
        {
            return null;
        }
        
        var mappedMap = _mapsMapper.Map(map);
        
        await _mapsDao.CreateAsync(mappedMap);

        return new MapDto(mappedMap.Id, map.Name, map.NorthLat, map.SouthLat, map.EastLon, map.WestLon, map.Url, map.FileId);
    }
}