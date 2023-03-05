namespace LibWebClient.Models;

/// <summary>
/// Server info
/// </summary>
public class ServerInfo
{
    /// <summary>
    /// Server name
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Protocol version
    /// </summary>
    public int ProtocolVersion { get; }

    public ServerInfo(string name, int protocolVersion)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException(nameof(name));
        }
        
        Name = name;
        ProtocolVersion = protocolVersion;
    }
}