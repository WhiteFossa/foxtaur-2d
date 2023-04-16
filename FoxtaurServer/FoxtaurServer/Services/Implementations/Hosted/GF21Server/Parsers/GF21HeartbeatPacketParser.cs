using System.Text.RegularExpressions;
using FoxtaurServer.Models.Trackers;
using FoxtaurServer.Models.Trackers.GF21;

namespace FoxtaurServer.Services.Implementations.Hosted.Parsers;

/// <summary>
/// Heartbeat package parser
/// </summary>
public class GF21HeartbeatPacketParser : IGF21Parser
{
    // Correct heartbeat packet regexp
    private const string HeartbeatPacketRegexp = @"^TRVYP16,[0-9]{32}#$";
    
    private readonly ILogger _logger;

    public GF21HeartbeatPacketParser(ILogger<GF21HeartbeatPacketParser> logger)
    {
        _logger = logger;
    }
    
    public GF21ParserResponse Parse(string message, TrackerContext context)
    {
        var match = Regex.Match(message, HeartbeatPacketRegexp, RegexOptions.IgnoreCase);
        if (!match.Success)
        {
            return new GF21ParserResponse(false, "");
        }

        _logger.LogWarning("Heartbeat packet.");
        
        return new GF21ParserResponse(true, @"TRVZP16#");
    }
}