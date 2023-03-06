namespace FoxtaurServer.Constants;

/// <summary>
/// Global server constants
/// </summary>
public class GlobalConstants
{
    #region Server info
    
    /// <summary>
    /// Protocol version
    /// </summary>
    public const int ProtocolVersion = 4;
    
    #endregion

    #region Configuration

    /// <summary>
    /// Take server name from this settings in appsettings.json
    /// </summary>
    public const string ServerNameSettingName = "Server:Name";

    #endregion
}