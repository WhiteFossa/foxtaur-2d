using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api.Requests;

/// <summary>
/// Parameters for teams mass get request
/// </summary>
public class TeamsMassGetRequest
{
    /// <summary>
    /// List of teams IDs to get teams
    /// </summary>
    [JsonPropertyName("teamsIds")]
    public IReadOnlyCollection<Guid> TeamsIds { get; set; }
}