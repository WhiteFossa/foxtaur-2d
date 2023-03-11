using FoxtaurServer.Dao.Models;
using FoxtaurServer.Mappers.Abstract;
using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Mappers.Implementations;

public class ProfilesMapper : IProfilesMapper
{
    private readonly IColorsMapper _colorsMapper;
    private readonly ITeamsMapper _teamsMapper;

    public ProfilesMapper(IColorsMapper colorsMapper,
        ITeamsMapper teamsMapper)
    {
        _colorsMapper = colorsMapper;
        _teamsMapper = teamsMapper;
    }
    
    public IReadOnlyCollection<ProfileDto> Map(IEnumerable<Profile> profiles)
    {
        if (profiles == null)
        {
            return null;
        }
        
        return profiles.Select(p => Map(p)).ToList();
    }

    public ProfileDto Map(Profile profile)
    {
        if (profile == null)
        {
            return null;
        }

        return new ProfileDto(
            Guid.Parse(profile.Id),
            profile.FirstName,
            profile.MiddleName,
            profile.LastName,
            profile.Sex,
            profile.DateOfBirth,
            profile.Phone,
            _teamsMapper.Map(profile.Team),
            profile.Category,
            _colorsMapper.Map(profile.ColorR, profile.ColorG, profile.ColorB, profile.ColorA));
    }

    public Profile Map(ProfileDto profile)
    {
        if (profile == null)
        {
            return null;
        }

        return new Profile()
        {
            Id = profile.Id.ToString(),
            FirstName = profile.FirstName,
            MiddleName = profile.MiddleName,
            LastName = profile.LastName,
            Sex = profile.Sex,
            DateOfBirth = profile.DateOfBirth,
            Phone = profile.Phone,
            Team = _teamsMapper.Map(profile.Team),
            Category = profile.Category,
            ColorR = profile.Color.R,
            ColorG = profile.Color.G,
            ColorB = profile.Color.B,
            ColorA = profile.Color.A
        };
    }

    public IReadOnlyCollection<Profile> Map(IEnumerable<ProfileDto> profiles)
    {
        if (profiles == null)
        {
            return null;
        }

        return profiles.Select(p => Map(p)).ToList();
    }
}