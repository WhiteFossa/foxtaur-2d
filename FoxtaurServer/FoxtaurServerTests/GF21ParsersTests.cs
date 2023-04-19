using FoxtaurServer.Models.Trackers;
using FoxtaurServer.Services.Abstract;
using FoxtaurServer.Services.Implementations.Hosted.Parsers;
using Microsoft.Extensions.Logging;
using Moq;

namespace FoxtaurServerTests;

public class GF21ParsersTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void LoginPacketTest()
    {
        var logger = Mock.Of<ILogger<GF21LoginPacketParser>>();
        
        var parser = new GF21LoginPacketParser(logger);

        var context = new TrackerContext();
        
        var correctMessage = @"TRVAP00353456789012345#";
        var result = parser.ParseAsync(correctMessage, context).Result;
        Assert.IsTrue(result.IsRecognized);
        Assert.AreEqual("353456789012345", context.Imei);

        var incorrectMessage = @"Yiff!Yuff!Yerf!";
        result = parser.ParseAsync(incorrectMessage, context).Result;
        Assert.IsFalse(result.IsRecognized);
    }

    [Test]
    public void LocationPacketTest()
    {
        var logger = Mock.Of<ILogger<GF21LocationPacketParser>>();
        var hunterLocationsService = Mock.Of<IHuntersLocationsService>();
        
        var parser = new GF21LocationPacketParser(logger, hunterLocationsService);
        
        var context = new TrackerContext();
        var correctMessage = @"TRVYP14080524A2232.9806N11404.9355E000.1061830323.870600090800010200011,460,0,9520,3671,Home|74-DE-2B-44-88-8C|97&Home1|74-DE-2B-44-88-8C|97&Home2|74-DE-2B-44-88-8C|97& Home3|74-DE-2B-44-88-8C|97#";
        var result = parser.ParseAsync(correctMessage, context).Result;
        Assert.IsTrue(result.IsRecognized);

        var incorrectMessage = @"Yiff!Yuff!Yerf!";
        result = parser.ParseAsync(incorrectMessage, context).Result;
        Assert.IsFalse(result.IsRecognized);
    }
    
    [Test]
    public void ShutdownPacketTest()
    {
        var logger = Mock.Of<ILogger<GF21ShutdownPacketParser>>();
        
        var parser = new GF21ShutdownPacketParser(logger);
        
        var context = new TrackerContext();
        var correctMessage = @"TRVAP89,000009,0,1#";
        var result = parser.ParseAsync(correctMessage, context).Result;
        Assert.IsTrue(result.IsRecognized);
        Assert.AreEqual(@"TRVBP89#", result.Response);

        var incorrectMessage = @"Yiff!Yuff!Yerf!";
        result = parser.ParseAsync(incorrectMessage, context).Result;
        Assert.IsFalse(result.IsRecognized);
    }
    
    [Test]
    public void HeartbeatPacketTest()
    {
        var logger = Mock.Of<ILogger<GF21HeartbeatPacketParser>>();
        
        var parser = new GF21HeartbeatPacketParser(logger);
        
        var context = new TrackerContext();
        var correctMessage = @"TRVYP16,10000909500020010000204000099992#";
        var result = parser.ParseAsync(correctMessage, context).Result;
        Assert.IsTrue(result.IsRecognized);
        Assert.AreEqual(@"TRVZP16#", result.Response);

        var incorrectMessage = @"Yiff!Yuff!Yerf!";
        result = parser.ParseAsync(incorrectMessage, context).Result;
        Assert.IsFalse(result.IsRecognized);
    }
}