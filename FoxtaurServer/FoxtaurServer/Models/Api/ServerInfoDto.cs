namespace FoxtaurServer.Models.Api;

/// <summary>
/// Server info
/// </summary>
public class ServerInfoDto
{
    /// <summary>
    /// Server name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Protocol version
    /// </summary>
    public int ProtocolVersion { get; }

    public ServerInfoDto(string name, int protocolVersion)
    {
        Name = name;
        ProtocolVersion = protocolVersion;
    }
}