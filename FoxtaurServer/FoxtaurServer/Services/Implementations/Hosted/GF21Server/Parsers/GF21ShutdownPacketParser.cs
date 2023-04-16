using System.Text.RegularExpressions;
using FoxtaurServer.Models.Trackers;
using FoxtaurServer.Models.Trackers.GF21;

namespace FoxtaurServer.Services.Implementations.Hosted.Parsers;

/// <summary>
/// It seems that this packet is a request to shutdown
/// </summary>
public class GF21ShutdownPacketParser : IGF21Parser
{
    // Correct shutdown packet regexp
    private const string ShutdownPacketRegexp = @"^TRVAP89";
    
    private readonly ILogger _logger;

    public GF21ShutdownPacketParser(ILogger<GF21ShutdownPacketParser> logger)
    {
        _logger = logger;
    }
    
    public GF21ParserResponse Parse(string message, TrackerContext context)
    {
        var match = Regex.Match(message, ShutdownPacketRegexp, RegexOptions.IgnoreCase);
        if (!match.Success)
        {
            return new GF21ParserResponse(false, "");
        }

        _logger.LogWarning("Shutdown request.");
        
        return new GF21ParserResponse(true, @"TRVBP89#");
    }
}