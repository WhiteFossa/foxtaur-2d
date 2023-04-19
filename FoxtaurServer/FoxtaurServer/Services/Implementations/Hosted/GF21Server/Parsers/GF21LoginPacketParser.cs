using System.Text.RegularExpressions;
using FoxtaurServer.Models.Trackers;
using FoxtaurServer.Models.Trackers.GF21;

namespace FoxtaurServer.Services.Implementations.Hosted.Parsers;

/// <summary>
/// Login packet parser
/// </summary>
public class GF21LoginPacketParser : IGF21Parser
{
    // Correct login packet regexp
    private const string LoginPacketRegexp = @"^TRVAP00([0-9]{15})#$";

    private readonly ILogger _logger;

    public GF21LoginPacketParser(ILogger<GF21LoginPacketParser> logger)
    {
        _logger = logger;
    }
    
    public async Task<GF21ParserResponse> ParseAsync(string message, TrackerContext context)
    {
        var match = Regex.Match(message, LoginPacketRegexp, RegexOptions.IgnoreCase);
        if (!match.Success)
        {
            return new GF21ParserResponse(false, "");
        }
        
        var imei = match.Groups[1].Value;
        context.SetImei(imei);
        
        var currentTime = DateTime.UtcNow;
        var response = $"TRVBP00{ currentTime.Year }{currentTime.Month:00}{currentTime.Day:00}{currentTime.Hour:00}{currentTime.Minute:00}{currentTime.Second:00}#";

        return new GF21ParserResponse(true, response);
    }
}