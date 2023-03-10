using System.Text.Json.Serialization;

namespace FoxtaurServer.Models.Api;

/// <summary>
/// Server info
/// </summary>
public class ServerInfoDto
{
    /// <summary>
    /// Server name
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; }

    /// <summary>
    /// Protocol version
    /// </summary>
    [JsonPropertyName("protocolVersion")]
    public int ProtocolVersion { get; }

    public ServerInfoDto(string name, int protocolVersion)
    {
        Name = name;
        ProtocolVersion = protocolVersion;
    }
}