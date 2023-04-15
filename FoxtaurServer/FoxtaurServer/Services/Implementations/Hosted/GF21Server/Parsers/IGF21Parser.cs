using FoxtaurServer.Models.Trackers.GF21;

namespace FoxtaurServer.Services.Implementations.Hosted.Parsers;

/// <summary>
/// All parsers must implement this interface
/// </summary>
public interface IGF21Parser
{
    /// <summary>
    /// Parse message from tracker
    /// </summary>
    GF21ParserResponse Parse(string message);
}