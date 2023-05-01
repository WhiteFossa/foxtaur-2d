using System.Text.Json.Serialization;

namespace LibWebClient.Models.DTOs;

/// <summary>
/// Server info
/// </summary>
public class ServerInfoDto
{
    /// <summary>
    /// Server name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; }

    /// <summary>
    /// Protocol version
    /// </summary>
    [JsonPropertyName("protocolVersion")]
    public int ProtocolVersion { get; set; }
}