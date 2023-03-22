using FoxtaurServer.Dao.Models;
using FoxtaurServer.Dao.Models.Enums;

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
    public const int ProtocolVersion = 8;
    
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

    #region New profile data

    /// <summary>
    /// First name for new profile
    /// </summary>
    public const string NewProfileFirstName = "Not specified";

    /// <summary>
    /// Middle name for new profile
    /// </summary>
    public const string NewProfileMiddleName = "Not specified";

    /// <summary>
    /// Last name for new profile
    /// </summary>
    public const string NewProfileLastName = "Not specified";
    
    /// <summary>
    /// Body sex for new profile
    /// </summary>
    public const BodySex NewProfileSex = BodySex.NotSpecified;

    /// <summary>
    /// Birth date for new profile
    /// </summary>
    public static readonly DateTime NewProfileDateOfBirth = DateTime.MinValue;

    /// <summary>
    /// Phone for new profile
    /// </summary>
    public const string NewProfilePhone = "Not specified";

    /// <summary>
    /// Category for new profile
    /// </summary>
    public static readonly Category NewProfileCategory = Category.NotSpecified;

    /// <summary>
    /// R-component for new profile color
    /// </summary>
    public const byte NewProfileColorR = 255;

    /// <summary>
    /// G-component for new profile color
    /// </summary>
    public const byte NewProfileColorG = 255;
    
    /// <summary>
    /// B-component for new profile color
    /// </summary>
    public const byte NewProfileColorB = 255;
    
    /// <summary>
    /// A-component for new profile color
    /// </summary>
    public const byte NewProfileColorA = 255;

    #endregion
}