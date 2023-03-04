using FoxtaurServer;
using Microsoft.AspNetCore;
using NLog;
using NLog.Config;
using NLog.Web;

public class Program
{
    public static void Main(string[] args)
    {
        // NLog: setup the logger first to catch all errors
        LogManager.Configuration = new XmlLoggingConfiguration("nlog.config");
        var logger = LogManager.GetCurrentClassLogger();
        try
        {
            BuildWebHost(args).Run();
        }
        catch (Exception e)
        {
            //NLog: catch setup errors
            logger.Error(e, "Stopped program because of exception");
            throw;
        }
    }

    public static IWebHost BuildWebHost(string[] args) =>
        WebHost.CreateDefaultBuilder(args) //note: this also adds the default MS loggers
            .UseStartup<Startup>()
            .UseNLog() // NLog: setup NLog for Dependency injection
            .Build();
}