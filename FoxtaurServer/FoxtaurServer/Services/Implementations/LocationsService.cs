using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Dao.Models.Enums;
using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;
using LibAuxiliary.Helpers;

namespace FoxtaurServer.Services.Implementations;

public class LocationsService : ILocationsService
{
    private readonly ILocationsDao _locationsDao;
    private readonly ILocationsMapper _locationsMapper;

    public LocationsService(ILocationsDao locationsDao,
        ILocationsMapper locationsMapper)
    {
        _locationsDao = locationsDao;
        _locationsMapper = locationsMapper;
    }

    public async Task<IReadOnlyCollection<LocationDto>> MassGetLocationsAsync(IReadOnlyCollection<Guid> locationsIds)
    {
        _ = locationsIds ?? throw new ArgumentNullException(nameof(locationsIds));

        return _locationsMapper.Map(await _locationsDao.GetLocationsAsync(locationsIds));
    }

    public async Task<IReadOnlyCollection<LocationDto>> GetAllLocationsAsync()
    {
        return _locationsMapper.Map(await _locationsDao.GetAllLocationsAsync());
    }

    public async Task<LocationDto> CreateNewLocationAsync(LocationDto location)
    {
        _ = location ?? throw new ArgumentNullException(nameof(location));
        
        var mappedLocation = _locationsMapper.Map(location);
        
        await _locationsDao.CreateAsync(mappedLocation);

        return new LocationDto(mappedLocation.Id, location.Name, location.Type, location.Lat, location.Lon, location.FoxId);
    }
}