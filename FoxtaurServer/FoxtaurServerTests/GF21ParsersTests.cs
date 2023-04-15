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
        
        var correctMessage = @"TRVAP00353456789012345#";
        var result = parser.Parse(correctMessage);
        Assert.IsTrue(result.IsRecognized);

        var incorrectMessage = @"Yiff!Yuff!Yerf!";
        result = parser.Parse(incorrectMessage);
        Assert.IsFalse(result.IsRecognized);
    }

    [Test]
    public void LocationPacketTest()
    {
        var logger = Mock.Of<ILogger<GF21LocationPacketParser>>();
        
        var parser = new GF21LocationPacketParser(logger);
        
        var correctMessage = @"TRVYP14080524A2232.9806N11404.9355E000.1061830323.870600090800010200011,460,0,9520,3671,Home|74-DE-2B-44-88-8C|97&Home1|74-DE-2B-44-88-8C|97&Home2|74-DE-2B-44-88-8C|97& Home3|74-DE-2B-44-88-8C|97#";
        var result = parser.Parse(correctMessage);
        Assert.IsTrue(result.IsRecognized);

        var incorrectMessage = @"Yiff!Yuff!Yerf!";
        result = parser.Parse(incorrectMessage);
        Assert.IsFalse(result.IsRecognized);
    }
}