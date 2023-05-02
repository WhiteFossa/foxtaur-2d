using Avalonia;
using Avalonia.ReactiveUI;
using System;
using System.IO;
using LibAuxiliary.Abstract;
using LibAuxiliary.Implementations;
using LibFoxtaurAdmin.Services.Abstract;
using LibFoxtaurAdmin.Services.Implementations;
using LibWebClient.Services.Abstract;
using LibWebClient.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Extensions.Logging;

namespace FoxtaurAdmin;

class Program
{
    private static IConfigurationRoot _configuration;
    
    /// <summary>
    /// Dependency injection service provider
    /// </summary>
    public static ServiceProvider Di { get; set; }
    
    // Initialization code. Don't use any Avalonia, third-party APIs or any
    // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
    // yet and stuff might break.
    [STAThread]
    public static void Main(string[] args)
    {
        // Configuration
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true);

        _configuration = builder.Build();
        
        // Preparing DI
        Di = ConfigureServices()
            .BuildServiceProvider();
        
        // Setting-up NLog
        LogManager.Configuration = new NLogLoggingConfiguration(_configuration.GetSection("NLog"));
        
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
    {
        return AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
    }

    // Setting up DI
    public static IServiceCollection ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton<IConfiguration>(_configuration);
        
        // Singletons
        services.AddSingleton<IConfigurationService, ConfigurationService>();
        services.AddSingleton<IWebClient, WebClient>();
        services.AddSingleton<IUserMessagesService, UserMessagesService>();

        // HTTP clients
        services.AddHttpClient<IWebClientRaw, WebClientRaw>();
        
        return services;
    }
}