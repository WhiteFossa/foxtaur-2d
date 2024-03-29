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
    public const int ProtocolVersion = 9;
    
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
    
    #region Trackers
    
    #region GF21
    
    /// <summary>
    /// Take GF21 server port from this setting
    /// </summary>
    public const string GF21PortSettingName = "Trackers:GF21_Port";
    
    /// <summary>
    /// Take GF21 server listener threads count from this setting
    /// </summary>
    public const string GF21ListenerThreadsCountSettingName = "Trackers:GF21_ListenerThreads";
    
    #endregion
    
    #endregion

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
    
    #region Map files

    /// <summary>
    /// Put map files here. TODO: Move to appsettings.json
    /// </summary>
    public const string MapFilesRootPath = "/opt/foxtaur/mapfiles";

    /// <summary>
    /// We split file name (actually GUID) into this size chunks and use chunks as directory names. It's for better search performance
    /// </summary>
    public const int MapFilesDirectoriesNameLength = 2;
    
    #endregion
}