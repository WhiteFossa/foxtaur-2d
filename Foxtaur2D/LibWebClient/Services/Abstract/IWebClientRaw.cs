using LibWebClient.Models.DTOs;
using LibWebClient.Models.Requests;

namespace LibWebClient.Services.Abstract;

/// <summary>
/// Low-level web client
/// </summary>
public interface IWebClientRaw
{
    /// <summary>
    /// Get information about the server
    /// </summary>
    Task<ServerInfoDto> GetServerInfoAsync();
    
    /// <summary>
    /// Get distance by ID. Throws ArgumentException if map with given ID is not found
    /// </summary>
    Task<DistanceDto> GetDistanceByIdAsync(Guid id);

    /// <summary>
    /// Lists all available distances
    /// </summary>
    Task<IReadOnlyCollection<DistanceDto>> ListDistancesAsync();
    
    /// <summary>
    /// Mass get hunters locations
    /// </summary>
    Task<Dictionary<Guid, IReadOnlyCollection<HunterLocationDto>>> MassGetHuntersLocationsAsync(HuntersLocationsMassGetRequest request);

    /// <summary>
    /// Mass get foxes
    /// </summary>
    Task<IReadOnlyCollection<FoxDto>> MassGetFoxesAsync(FoxesMassGetRequest request);

    /// <summary>
    /// Mass get teams
    /// </summary>
    Task<IReadOnlyCollection<TeamDto>> MassGetTeamsAsync(TeamsMassGetRequest request);
    
    /// <summary>
    /// Mass get maps
    /// </summary>
    Task<IReadOnlyCollection<MapDto>> MassGetMapsAsync(MapsMassGetRequest request);
    
    /// <summary>
    /// Mass get hunters
    /// </summary>
    Task<IReadOnlyCollection<HunterDto>> MassGetHuntersAsync(HuntersMassGetRequest request);

    /// <summary>
    /// Mass get locations
    /// </summary>
    Task<IReadOnlyCollection<LocationDto>> MassGetLocationsAsync(LocationsMassGetRequest request);

    /// <summary>
    /// Makes HEAD request to given URI
    /// </summary>
    Task<HttpResponseMessage> GetHeadersAsync(Uri uri);

    /// <summary>
    /// Downloads part of file using GET with Range
    /// </summary>
    Task<HttpResponseMessage> DownloadWithRangeAsync(Uri uri, long start, long end);
}