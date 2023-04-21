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
    /// If true and IsRecognized is true too, then send to client message from Response
    /// </summary>
    public bool IsSendResponse { get; }

    /// <summary>
    /// If message was recognized and IsSendResponse is true, server must send this response to GF21
    /// </summary>
    public string Response { get; }

    public GF21ParserResponse(bool isRecognized, bool isSendResponse, string response)
    {
        IsRecognized = isRecognized;
        IsSendResponse = isSendResponse;
        Response = response;
    }
}