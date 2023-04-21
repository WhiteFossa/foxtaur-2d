using System.Text.RegularExpressions;
using FoxtaurServer.Models.Trackers;
using FoxtaurServer.Models.Trackers.GF21;

namespace FoxtaurServer.Services.Implementations.Hosted.Parsers;

/// <summary>
/// Parsing IMSI and ICCID identification packet. We don't need it, but protocol requires answer to it.
/// </summary>
public class GF21ImsiIccidPacketParser : IGF21Parser
{
    private const string ImsiIccidPacketRegexp = @"^TRVYP02,[0-9]{15},[0-9a-zA-Z]{20}#$";
    
    private readonly ILogger _logger;

    public GF21ImsiIccidPacketParser(ILogger<GF21ImsiIccidPacketParser> logger)
    {
        _logger = logger;
    }
    
    public async Task<GF21ParserResponse> ParseAsync(string message, TrackerContext context)
    {
        var match = Regex.Match(message, ImsiIccidPacketRegexp, RegexOptions.IgnoreCase);
        if (!match.Success)
        {
            return new GF21ParserResponse(false, false, "");
        }

        return new GF21ParserResponse(true, true, @"TRVZP02#");
    }
}