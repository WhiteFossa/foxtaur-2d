using Avalonia.Media;
using LibWebClient.Constants;
using LibWebClient.Enums;
using LibWebClient.Models;
using LibWebClient.Models.DTOs;
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
                    new List<Location>(),
                    new List<Hunter>(),
                    d.FirstHunterStartTime
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

        var startDto = await _client.GetLocationByIdAsync(distanceDto.StartLocationId).ConfigureAwait(false);
        if (startDto == null)
        {
            throw new InvalidOperationException($"Start location (ID={ distanceDto.StartLocationId }) is not found!");
        }
        
        var finishCorridorEntranceDto = await _client.GetLocationByIdAsync(distanceDto.FinishCorridorEntranceLocationId).ConfigureAwait(false);
        if (finishCorridorEntranceDto == null)
        {
            throw new InvalidOperationException($"Finish corridor entrance location (ID={ distanceDto.FinishCorridorEntranceLocationId }) is not found!");
        }
        
        var finishDto = await _client.GetLocationByIdAsync(distanceDto.FinishLocationId).ConfigureAwait(false);
        if (finishDto == null)
        {
            throw new InvalidOperationException($"Finish location (ID={ distanceDto.FinishLocationId }) is not found!");
        }

        // Foxes locations
        var foxesLocationsIds = distanceDto
            .FoxesLocationsIds;

        var foxesLocationsDtos = new List<LocationDto>();
        foreach (var foxLocationId in foxesLocationsIds)
        {
            foxesLocationsDtos.Add(await _client.GetLocationByIdAsync(foxLocationId).ConfigureAwait(false));
        }
        
        // Expected foxes locations order
        var expectedFoxesOrderLocationsIds = distanceDto
            .ExpectedFoxesOrderLocationsIds;
        var expectedFoxesOrderLocationsDtos = new List<LocationDto>();
        foreach (var expectedFoxOrderLocationId in expectedFoxesOrderLocationsIds)
        {
            expectedFoxesOrderLocationsDtos.Add(await _client.GetLocationByIdAsync(expectedFoxOrderLocationId).ConfigureAwait(false));
        }
        
        // Foxes
        var foxesIds = foxesLocationsDtos
            .Select(fl => fl.FoxId.Value)
            .ToList();
        var foxes = await MassGetFoxesAsync(new FoxesMassGetRequest(foxesIds)).ConfigureAwait(false);
        
        // Hunters
        var huntersIds = distanceDto
            .HuntersIds;
        
        var huntersDtos = new List<HunterDto>();
        foreach (var hunterId in huntersIds)
        {
            huntersDtos.Add(await _client.GetHunterByIdAsync(hunterId).ConfigureAwait(false));
        }
        
        // Hunters locations histories
        var huntersLocationsHistories = await _client.MassGetHuntersLocationsAsync(
            new HuntersLocationsMassGetRequest(
                huntersIds, distanceDto.FirstHunterStartTime)).ConfigureAwait(false);
        
        // Teams
        var teamsIds = huntersDtos
            .Where(h => h.TeamId.HasValue)
            .Select(h => h.TeamId.Value)
            .ToList();
        var teams = await MassGetTeamsAsync(new TeamsMassGetRequest(teamsIds)).ConfigureAwait(false);

        return new Distance(
            distanceDto.Id,
            distanceDto.Name,
            map,
            distanceDto.IsActive,
            new Location(startDto.Id, startDto.Name, startDto.Type, startDto.Lat, startDto.Lon, null),
            new Location(finishCorridorEntranceDto.Id, finishCorridorEntranceDto.Name, finishCorridorEntranceDto.Type, finishCorridorEntranceDto.Lat, finishCorridorEntranceDto.Lon, null),
            new Location(finishDto.Id, finishDto.Name, finishDto.Type, finishDto.Lat, finishDto.Lon, null),
            foxesLocationsDtos.Select(fl =>
            {
                var fox = foxes.Single(f => f.Id == fl.FoxId);
                
                return new Location(fl.Id, fl.Name, LocationType.Fox, fl.Lat, fl.Lon, fox);
            }).ToList(),
            expectedFoxesOrderLocationsDtos.Select(efol =>
            {
                var fox = foxes.Single(f => f.Id == efol.FoxId);

                return new Location(efol.Id, efol.Name, LocationType.Fox, efol.Lat, efol.Lon, fox);
            }).ToList(),
            huntersDtos.Select(h =>
            {
                var team = teams
                    .FirstOrDefault(td => td.Id == h.TeamId);

                return new Hunter(h.Id,
                    h.Name,
                    h.IsRunning,
                    team,
                    huntersLocationsHistories[h.Id].Select(hlh => new HunterLocation(hlh.Id, hlh.Timestamp, hlh.Lat, hlh.Lon, hlh.Alt)).ToList(),
                    new Color(h.Color.A, h.Color.R, h.Color.G, h.Color.B));
            }).ToList(),
            distanceDto.FirstHunterStartTime);
    }

    public async Task<Hunter> GetHunterByIdAsync(Guid hunterId, DateTime loadLocationsFrom)
    {
        var hunterDto = await _client.GetHunterByIdAsync(hunterId).ConfigureAwait(false);

        var hunterLocationsHistoryDto = (await _client.MassGetHuntersLocationsAsync(
            new HuntersLocationsMassGetRequest(
                new List<Guid>() { hunterId }, loadLocationsFrom)).ConfigureAwait(false))[hunterId];
            
        var team = hunterDto.TeamId.HasValue
            ? (await MassGetTeamsAsync(new TeamsMassGetRequest(new List<Guid>() { hunterDto.TeamId.Value })).ConfigureAwait(false)).First()
            : null;

        return new Hunter(hunterDto.Id,
            hunterDto.Name,
            hunterDto.IsRunning,
            team,
            hunterLocationsHistoryDto.Select(hlh => new HunterLocation(hlh.Id, hlh.Timestamp, hlh.Lat, hlh.Lon, hlh.Alt)).ToList(),
            new Color(hunterDto.Color.A, hunterDto.Color.R, hunterDto.Color.G, hunterDto.Color.B));
    }

    public async Task<Dictionary<Guid, IReadOnlyCollection<HunterLocation>>> MassGetHuntersLocationsAsync(HuntersLocationsMassGetRequest request)
    {
        _ = request ?? throw new ArgumentNullException(nameof(request));
        
        var locationsDictionary = await _client.MassGetHuntersLocationsAsync(request).ConfigureAwait(false);

        return new Dictionary<Guid, IReadOnlyCollection<HunterLocation>>(locationsDictionary
            .Select(l => new KeyValuePair<Guid, IReadOnlyCollection<HunterLocation>>(l.Key, l
                .Value
                .Select(hl => new HunterLocation(hl.Id, hl.Timestamp, hl.Lat, hl.Lon, hl.Alt))
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
}