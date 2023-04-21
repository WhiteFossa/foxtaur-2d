using FoxtaurServer.Models.Trackers;

namespace FoxtaurServer.Services.Implementations.Hosted.Commands;

public class GF21SetStationarySleepCommand : IGF21Command
{
    private Random _random = new Random();

    /// <summary>
    /// If true, then enable sleepmode, if false, then disable
    /// </summary>
    private bool _isSleep;

    public GF21SetStationarySleepCommand(bool isSleep)
    {
        _isSleep = isSleep;
    }
    
    public async Task<string> SendCommandAsync(TrackerContext context)
    {
        _ = context ?? throw new ArgumentNullException(nameof(context), "Tracker context can't be null.");
        
        var commandId = $"{_random.Next(1, 999_999):000000}";
        context.SetLastCommandId(commandId);

        var isSleepCode = _isSleep ? "1" : "0";
        
        return $"TRVDP21,{ commandId },{ isSleepCode }#";
    }
}