using System.Text.Json.Serialization;

namespace LibWebClient.Models.Requests;

/// <summary>
/// Parameters for teams mass get request
/// </summary>
public class TeamsMassGetRequest
{
    /// <summary>
    /// List of teams IDs to get teams
    /// </summary>
    [JsonPropertyName("teamsIds")]
    public IReadOnlyCollection<Guid> TeamsIds { get; }

    public TeamsMassGetRequest(IReadOnlyCollection<Guid> teamsIds)
    {
        TeamsIds = teamsIds ?? throw new ArgumentNullException(nameof(teamsIds));
    }
}