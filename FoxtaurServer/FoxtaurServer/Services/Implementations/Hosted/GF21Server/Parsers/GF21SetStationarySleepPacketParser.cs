using System.Text.RegularExpressions;
using FoxtaurServer.Models.Trackers;
using FoxtaurServer.Models.Trackers.GF21;

namespace FoxtaurServer.Services.Implementations.Hosted.Parsers;

/// <summary>
/// Parses the response of GF21SetStationarySleepCommand
/// </summary>
public class GF21SetStationarySleepPacketParser : IGF21Parser
{
    // Correct heartbeat packet regexp
    private const string SetStationarySleepModeResponsePacketRegexp = @"^TRVDP21,([0-9]{6}),([0-9])#$";
    
    private readonly ILogger _logger;

    public GF21SetStationarySleepPacketParser(ILogger<GF21SetStationarySleepPacketParser> logger)
    {
        _logger = logger;
    }
    
    public async Task<GF21ParserResponse> ParseAsync(string message, TrackerContext context)
    {
        var match = Regex.Match(message, SetStationarySleepModeResponsePacketRegexp, RegexOptions.IgnoreCase);
        if (!match.Success)
        {
            return new GF21ParserResponse(false, false, "");
        }
        
        var lastCommandId = match.Groups[1].Value;
        var resultCode = match.Groups[2].Value;

        if (!lastCommandId.Equals(context.LastCommandId))
        {
            // We've got the correct answer, but it seems that to another instance of this command
            _logger.LogWarning($"Tracker { context.Imei } - Wrong last command ID! Expected { context.LastCommandId }, got { lastCommandId }.");
            
            return new GF21ParserResponse(true, false, "");
        }

        var sleepModeString = resultCode.Equals("1") ? "enabled" : "disabled";
        _logger.LogWarning($"Tracker { context.Imei } - Sleepmode { sleepModeString }.");
        
        return new GF21ParserResponse(true, false, "");
    }
}