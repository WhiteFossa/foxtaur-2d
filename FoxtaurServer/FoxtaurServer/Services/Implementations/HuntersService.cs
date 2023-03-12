using FoxtaurServer.Dao.Abstract;
using FoxtaurServer.Dao.Models;
using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Models.Api;
using FoxtaurServer.Models.Api.Enums;
using FoxtaurServer.Models.Api.Requests;
using FoxtaurServer.Services.Abstract;

namespace FoxtaurServer.Services.Implementations;

public class HuntersService : IHuntersService
{
    private readonly IProfilesDao _profilesDao;
    private readonly IProfilesMapper _profilesMapper;
    private readonly IColorsMapper _colorsMapper;
    private readonly IDistancesDao _distancesDao;
    
    public HuntersService(IProfilesDao profilesDao,
        IProfilesMapper profilesMapper,
        IColorsMapper colorsMapper,
        IDistancesDao distancesDao)
    {
        _profilesDao = profilesDao;
        _profilesMapper = profilesMapper;
        _colorsMapper = colorsMapper;
        _distancesDao = distancesDao;
    }

    public async Task<IReadOnlyCollection<HunterDto>> MassGetHuntersAsync(IReadOnlyCollection<Guid> huntersIds)
    {
        _ = huntersIds ?? throw new ArgumentNullException(nameof(huntersIds));

        var profiles = await _profilesDao.GetProfilesAsync(huntersIds.Select(hid => hid.ToString()).ToList());
        
        return profiles
            .Select(p => new HunterDto(Guid.Parse(p.Id), GetHunterFullName(p), true, p.Team?.Id, _colorsMapper.Map(p.ColorR, p.ColorG, p.ColorB, p.ColorA))) // TODO: Add "Is Hunter Running" detection code
            .ToList();
    }

    private string GetHunterFullName(Profile profile)
    {
        return $"{ profile.FirstName } { profile.MiddleName } { profile.LastName }"; // TODO: Add more sophisticated logic
    }

    public async Task<IReadOnlyCollection<ProfileDto>> MassGetHuntersProfilesAsync(IReadOnlyCollection<Guid> huntersIds)
    {
        _ = huntersIds ?? throw new ArgumentNullException(nameof(huntersIds));

        return _profilesMapper.Map(await _profilesDao.GetProfilesAsync(huntersIds.Select(hid => hid.ToString()).ToList()));
    }

    public async Task<ProfileDto> UpdateHunterProfileAsync(ProfileUpdateRequest request, Guid hunterId)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var profile = new Profile()
        {
            Id = hunterId.ToString(),
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
            LastName = request.LastName,
            Sex = request.Sex,
            DateOfBirth = request.DateOfBirth,
            Phone = request.Phone,
            Team = new Team() { Id = request.TeamId },
            Category = request.Category,
            ColorR = request.Color.R,
            ColorG = request.Color.G,
            ColorB = request.Color.B,
            ColorA = request.Color.A
        };

        await _profilesDao.UpdateAsync(profile);
        
        return _profilesMapper.Map(profile);
    }

    public async Task<RegistrationOnDistanceResponseDto> RegisterOnDistanceAsync(RegisterOnDistanceRequest request, Guid hunterId)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var distance = await _distancesDao.GetDistanceByIdAsync(request.DistanceId);
        if (distance == null)
        {
            return new RegistrationOnDistanceResponseDto(RegistrationOnDistanceResult.DistanceNotFound);
        }

        if (distance.Hunters.Select(h => h.Id).Contains(hunterId.ToString()))
        {
            // Hunter already registered on this distance
            return new RegistrationOnDistanceResponseDto(RegistrationOnDistanceResult.AlreadyRegistered);
        }
        
        distance.Hunters.Add(new Profile() { Id = hunterId.ToString() });

        try
        {
            await _distancesDao.UpdateAsync(distance);
        }
        catch (Exception)
        {
            return new RegistrationOnDistanceResponseDto(RegistrationOnDistanceResult.Failure);
        }
        
        return new RegistrationOnDistanceResponseDto(RegistrationOnDistanceResult.Success);
    }
}