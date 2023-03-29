using Avalonia.Media;
using LibWebClient.Constants;
using LibWebClient.Enums;
using LibWebClient.Models;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;
using NLog;

namespace LibWebClient.Services.Implementations;

public class WebClient : IWebClient
{
    private readonly Logger _logger = LogManager.GetCurrentClassLogger();
    
    private readonly IWebClientRaw _client;

    public WebClient(IWebClientRaw webClient)
    {
        _client = webClient;
        
        // Querying information about the server
        var serverInfo = _client.GetServerInfoAsync().Result;
        
        _logger.Info($"Server name: { serverInfo.Name }");
        _logger.Info($"Protocol version: { serverInfo.ProtocolVersion }");

        if (WebClientConstants.ProtocolVersion != serverInfo.ProtocolVersion)
        {
            _logger.Error($"Protocol version mismatch. Expected { WebClientConstants.ProtocolVersion }, got { serverInfo.ProtocolVersion }.");
            _logger.Error("Client update required!");

            throw new InvalidOperationException();
        }
    }
    
    public async Task<IReadOnlyCollection<Distance>> GetDistancesWithoutIncludeAsync()
    {
        var distances = await _client.ListDistancesAsync().ConfigureAwait(false);

        var mapsIds = distances
            .Select(d => d.MapId)
            .Distinct()
            .ToList();
        var maps = await _client.MassGetMapsAsync(new MapsMassGetRequest(mapsIds)).ConfigureAwait(false);
        
        return distances
            .Select(d =>
            {
                var mapDto = maps.Single(m => m.Id == d.MapId);
                
                return new Distance(
                    d.Id,
                    d.Name,
                    new Map(mapDto.Id, mapDto.Name, mapDto.NorthLat, mapDto.SouthLat, mapDto.EastLon, mapDto.WestLon, mapDto.Url),
                    d.IsActive,
                    new Location(Guid.NewGuid(), "Invalid start location", LocationType.Start, 0, 0, null),
                    new Location(Guid.NewGuid(), "Invalid finish corridor entrance location", LocationType.FinishCorridorEntrance, 0, 0, null),
                    new Location(Guid.NewGuid(), "Invalid finish location", LocationType.Start, 0, 0, null),
                    new List<Location>(),
                    new List<Hunter>(),
                    d.FirstHunterStartTime,
                    d.CloseTime
                );
            })
            .ToList();
    }

    public async Task<Distance> GetDistanceByIdAsync(Guid distanceId)
    {
        var distanceDto = await _client.GetDistanceByIdAsync(distanceId).ConfigureAwait(false);
        if (distanceDto == null)
        {
            throw new ArgumentException(nameof(distanceId));
        }

        var map = (await MassGetMapsAsync(new MapsMassGetRequest(new List<Guid>() { distanceDto.MapId })).ConfigureAwait(false))
            .Single();

        var start = (await MassGetLocationsAsync(new LocationsMassGetRequest(new List<Guid>() { distanceDto.StartLocationId })).ConfigureAwait(false))
            .Single();

        var finishCorridorEntrance = (await MassGetLocationsAsync(new LocationsMassGetRequest(new List<Guid>() { distanceDto.FinishCorridorEntranceLocationId })).ConfigureAwait(false))
            .Single();

        var finish = (await MassGetLocationsAsync(new LocationsMassGetRequest(new List<Guid>() { distanceDto.FinishLocationId })).ConfigureAwait(false))
            .Single();
        
        // Foxes locations
        var foxesLocationsIds = distanceDto
            .FoxesLocationsIds;
        var foxesLocations = await MassGetLocationsAsync(new LocationsMassGetRequest(foxesLocationsIds)).ConfigureAwait(false);
        
        // Hunters
        var huntersIds = distanceDto
            .HuntersIds;
        var hunters = await MassGetHuntersAsync(new HuntersMassGetRequest(huntersIds), distanceDto.FirstHunterStartTime, distanceDto.CloseTime).ConfigureAwait(false);
        
        return new Distance(
            distanceDto.Id,
            distanceDto.Name,
            map,
            distanceDto.IsActive,
            start,
            finishCorridorEntrance,
            finish,
            foxesLocations,
            hunters,
            distanceDto.FirstHunterStartTime,
            distanceDto.CloseTime);
    }

    public async Task<Dictionary<Guid, IReadOnlyCollection<HunterLocation>>> MassGetHuntersLocationsAsync(HuntersLocationsMassGetRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));
        
        var locationsDictionary = await _client.MassGetHuntersLocationsAsync(request).ConfigureAwait(false);

        return new Dictionary<Guid, IReadOnlyCollection<HunterLocation>>(locationsDictionary
            .Select(l => new KeyValuePair<Guid, IReadOnlyCollection<HunterLocation>>(l.Key, l
                .Value
                .Select(hl => new HunterLocation(hl.Id, hl.Timestamp, hl.Lat, hl.Lon, hl.Alt))
                .OrderBy(hl => hl.Timestamp)
                .ToList())));
    }

    public async Task<IReadOnlyCollection<Fox>> MassGetFoxesAsync(FoxesMassGetRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var foxes = await _client.MassGetFoxesAsync(request).ConfigureAwait(false);

        return foxes
            .Select(f => new Fox(f.Id, f.Name, f.Frequency, f.Code))
            .ToList();
    }

    public async Task<IReadOnlyCollection<Team>> MassGetTeamsAsync(TeamsMassGetRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var teams = await _client.MassGetTeamsAsync(request).ConfigureAwait(false);

        return teams
            .Select(t => new Team(t.Id, t.Name, new Color(t.Color.A, t.Color.R, t.Color.G, t.Color.B)))
            .ToList();
    }

    public async Task<IReadOnlyCollection<Map>> MassGetMapsAsync(MapsMassGetRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var maps = await _client.MassGetMapsAsync(request).ConfigureAwait(false);

        return maps
            .Select(m => new Map(m.Id, m.Name, m.NorthLat, m.SouthLat, m.EastLon, m.WestLon, m.Url))
            .ToList();
    }

    public async Task<IReadOnlyCollection<Hunter>> MassGetHuntersAsync(HuntersMassGetRequest request, DateTime locationsHistoriesFromTime, DateTime locationsHistoriesToTime)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var hunters = await _client.MassGetHuntersAsync(request).ConfigureAwait(false);
        
        var teamsIds = hunters
            .Where(h => h.TeamId.HasValue)
            .Select(h => h.TeamId.Value)
            .Distinct()
            .ToList();
        var teams = await MassGetTeamsAsync(new TeamsMassGetRequest(teamsIds)).ConfigureAwait(false);

        var locationsHistories = await MassGetHuntersLocationsAsync(new HuntersLocationsMassGetRequest
            (
                request.HuntersIds,
                locationsHistoriesFromTime,
                locationsHistoriesToTime
            )).ConfigureAwait(false);

        return hunters
            .Select(h => new Hunter(
                h.Id,
                h.Name,
                h.IsRunning,
                h.TeamId != null ? teams.Single(t => t.Id == h.TeamId.Value) : null,
                locationsHistories.ContainsKey(h.Id) ? locationsHistories[h.Id] : new List<HunterLocation>(),
                new Color(h.Color.A, h.Color.R, h.Color.G, h.Color.B)))
            .ToList();
    }

    public async Task<IReadOnlyCollection<Location>> MassGetLocationsAsync(LocationsMassGetRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));

        var locations = await _client.MassGetLocationsAsync(request).ConfigureAwait(false);

        var foxesIds = locations
            .Where(l => l.FoxId.HasValue)
            .Select(l => l.FoxId.Value)
            .ToList();
        var foxes = await MassGetFoxesAsync(new FoxesMassGetRequest(foxesIds)).ConfigureAwait(false);

        return locations
            .Select(l =>
            {
                var fox = l.FoxId.HasValue
                    ? foxes.Single(f => f.Id == l.FoxId.Value)
                    : null;
                
                return new Location(l.Id, l.Name, l.Type, l.Lat, l.Lon, fox);
            })
            .ToList();
    }

    public async Task<IReadOnlyCollection<Hunter>> MassGetHuntersByDistanceIdWithoutLocationsHistoriesAsync(Guid distanceId)
    {
        var distanceDto = await _client.GetDistanceByIdAsync(distanceId).ConfigureAwait(false);
        if (distanceDto == null)
        {
            throw new ArgumentException(nameof(distanceId));
        }
        
        // Hunters
        var huntersIds = distanceDto
            .HuntersIds;
        
        var hunters = await _client.MassGetHuntersAsync(new HuntersMassGetRequest(huntersIds)).ConfigureAwait(false);
        
        // Teams
        var teamsIds = hunters
            .Where(h => h.TeamId.HasValue)
            .Select(h => h.TeamId.Value)
            .Distinct()
            .ToList();
        var teams = await MassGetTeamsAsync(new TeamsMassGetRequest(teamsIds)).ConfigureAwait(false);

        return hunters
            .Select(h => new Hunter(
                h.Id,
                h.Name,
                h.IsRunning,
                h.TeamId != null ? teams.Single(t => t.Id == h.TeamId.Value) : null,
                new List<HunterLocation>(),
                new Color(h.Color.A, h.Color.R, h.Color.G, h.Color.B)))
            .ToList();
    }

    public async Task<HttpResponseMessage> GetHeadersAsync(Uri uri)
    {
        _ = uri ?? throw new ArgumentNullException(nameof(uri));

        return await _client.GetHeadersAsync(uri).ConfigureAwait(false);
    }
}