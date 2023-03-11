using FoxtaurServer.Dao.Models;
using FoxtaurServer.Models.Api;

namespace FoxtaurServer.Mappers.Abstract;

/// <summary>
/// Mapper to map profiles
/// </summary>
public interface IProfilesMapper
{
    IReadOnlyCollection<ProfileDto> Map(IEnumerable<Profile> profiles);

    ProfileDto Map(Profile profile);

    Profile Map(ProfileDto profile);

    IReadOnlyCollection<Profile> Map(IEnumerable<ProfileDto> profiles);
}