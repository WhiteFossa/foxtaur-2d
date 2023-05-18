using System.Text.Json.Serialization;

namespace LibWebClient.Models.Requests;

/// <summary>
/// Mark map file as ready request
/// </summary>
public class MarkMapFileAsReadyRequest
{
    /// <summary>
    /// Map file ID
    /// </summary>
    [JsonPropertyName("id")]
    public Guid Id { get; }

    public MarkMapFileAsReadyRequest(Guid id)
    {
        Id = id;
    }
}