namespace LibWebClient.Constants;

/// <summary>
/// Constants, related to web client
/// </summary>
public static class WebClientConstants
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
    /// Reautentificate on server if session remaining time is less than this value
    /// </summary>
    public static readonly TimeSpan ReauthentificateBefore = new TimeSpan(1, 0, 0);
}