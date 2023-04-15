using System.Text.RegularExpressions;
using FoxtaurServer.Models.Trackers.GF21;

namespace FoxtaurServer.Services.Implementations.Hosted.Parsers;

// Location packet parser
public class GF21LocationPacketParser : IGF21Parser
{
    /// <summary>
    /// Location package regexp
    /// </summary>
    private const string LocationPacketRegexp = @"^TRVYP14([0-9]{2})([0-9]{2})([0-9]{2})([A-Z])([0-9]{2})([0-9]{2}.[0-9]{4})([A-Z])([0-9]{3})([0-9]{2}.[0-9]{4})([A-Z])([0-9]{3}.[0-9])([0-9]{2})([0-9]{2})([0-9]{2})";

    /// <summary>
    /// Response to correct message
    /// </summary>
    private const string Response = @"TRVZP14#";
    
    private readonly ILogger _logger;

    public GF21LocationPacketParser(ILogger<GF21LocationPacketParser> logger)
    {
        _logger = logger;
    }
    
    public GF21ParserResponse Parse(string message)
    {
        var match = Regex.Match(message, LocationPacketRegexp, RegexOptions.IgnoreCase);
        if (!match.Success)
        {
            return new GF21ParserResponse(false, "");
        }
        
        var year = match.Groups[1].Value;
        var month = match.Groups[2].Value;
        var day = match.Groups[3].Value;
        
        var validityMarker = match.Groups[4].Value;
        
        var latDegrees = match.Groups[5].Value;
        var latMinutes = match.Groups[6].Value;
        var latSign = match.Groups[7].Value;
        
        var lonDegrees = match.Groups[8].Value;
        var lonMinutes = match.Groups[9].Value;
        var lonSign = match.Groups[10].Value;

        var hour = match.Groups[12].Value;
        var minute = match.Groups[13].Value;
        var second = match.Groups[14].Value;

        var locationDateTime = new DateTime(2000 + int.Parse(year),
            int.Parse(month),
            int.Parse(day),
            int.Parse(hour),
            int.Parse(minute),
            int.Parse(second));
        
        _logger.LogWarning($"Location time: { locationDateTime }");

        var isLocationValid = validityMarker == "A";
        if (!isLocationValid)
        {
            // Location invalid
            _logger.LogWarning("Location invalid.");
            return new GF21ParserResponse(true, Response);
        }

        var lat = (double)int.Parse(latDegrees);
        lat += double.Parse(latMinutes) * 100.0 / 6000.0;
        if (latSign == "S")
        {
            lat *= -1;
        }
        
        _logger.LogWarning($"Lat: { lat }");
        
        var lon = (double)int.Parse(lonDegrees);
        lon += double.Parse(lonMinutes) * 100.0 / 6000.0;
        if (lonSign == "W")
        {
            lon *= -1;
        }
        
        _logger.LogWarning($"Lon: { lon }");
        
        return new GF21ParserResponse(true, Response);
    }
}