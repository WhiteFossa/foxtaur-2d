namespace LibWebClient.Constants;

/// <summary>
/// Constants, related to web client
/// </summary>
public class WebClientConstants
{
    /// <summary>
    /// Client able to work with this protocol version
    /// </summary>
    public const int ProtocolVersion = 9;

    /// <summary>
    /// HTTP Client timeout (in seconds)
    /// </summary>
    public const int HttpClientTimeout = 240;

    /// <summary>
    /// HTTP Client timeout for downloads (in seconds)
    /// </summary>
    public const int HttpClientDownloadTimeout = 3600;
}