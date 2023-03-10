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
    public const int ProtocolVersion = 6;
    
    #endregion

    #region Configuration

    /// <summary>
    /// Take server name from this settings in appsettings.json
    /// </summary>
    public const string ServerNameSettingName = "Server:Name";
    
    /// <summary>
    /// Take JWT secret from this settings in appsettings.json
    /// </summary>
    public const string JwtSecretSettingName = "JWT:Secret";
    
    /// <summary>
    /// Take JWT valid issuer from this settings in appsettings.json
    /// </summary>
    public const string JwtValidIssuerSettingName = "JWT:ValidIssuer";
    
    /// <summary>
    /// Take JWT valid audience from this settings in appsettings.json
    /// </summary>
    public const string JwtValidAudienceSettingName = "JWT:ValidAudience";

    #endregion
    
    #region Security

    /// <summary>
    /// JWT lifetime in hours
    /// </summary>
    public const int JwtLifetime = 24;

    #endregion
}