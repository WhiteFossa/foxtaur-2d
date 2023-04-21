using FoxtaurServer.Models.Identity;
using FoxtaurServer.Models.Trackers;
using FoxtaurServer.Services.Implementations.Hosted.Commands;

namespace FoxtaurServerTests;

public class GF21CommandsTests
{
    [SetUp]
    public void Setup()
    {
    }
    
    [Test]
    public void SetSleepModeTest()
    {
        var context = new TrackerContext();

        var enableSleepModeCommand = new GF21SetStationarySleepCommand(true);
        var enableSleepModeMessage = enableSleepModeCommand.SendCommandAsync(context).Result;
        Assert.AreEqual($"TRVDP21,{ context.LastCommandId },1#", enableSleepModeMessage);
        
        var disableSleepModeCommand = new GF21SetStationarySleepCommand(false);
        var disableSleepModeMessage = disableSleepModeCommand.SendCommandAsync(context).Result;
        Assert.AreEqual($"TRVDP21,{ context.LastCommandId },0#", disableSleepModeMessage);
    }
}