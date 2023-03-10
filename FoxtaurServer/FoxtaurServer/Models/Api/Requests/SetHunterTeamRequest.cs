using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api.Requests;

/// <summary>
/// Parameters for setting team for hunter
/// </summary>
public class SetHunterTeamRequest
{
    /// <summary>
    /// Assign team to a hunter. Team might be null, in this case hunter will be teamless
    /// </summary>
    [JsonPropertyName("teamId")]
    public Guid? TeamId { get; set; }
}