using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api.Requests;

/// <summary>
/// Mark map file as ready request
/// </summary>
public class MarkMapFileAsReadyRequest
{
    /// <summary>
    /// Map file ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}