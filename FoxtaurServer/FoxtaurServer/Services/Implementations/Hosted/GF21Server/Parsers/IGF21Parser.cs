using FoxtaurServer.Models.Trackers;
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
    Task<GF21ParserResponse> ParseAsync(string message, TrackerContext context);
}