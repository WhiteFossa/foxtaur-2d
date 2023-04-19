namespace FoxtaurServer.Models.Trackers.GF21;

/// <summary>
/// Response from GF21 message parsers
/// </summary>
public class GF21ParserResponse
{
    /// <summary>
    /// If true, then parser recognized a message from GF21
    /// </summary>
    public bool IsRecognized { get; }
    
    /// <summary>
    /// If message was parsed, server must send this response to GF21
    /// </summary>
    public string Response { get; }

    public GF21ParserResponse(bool isRecognized, string response)
    {
        IsRecognized = isRecognized;
        Response = response;
    }
}