﻿using System;
using System.IO;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.ReactiveUI;
using Foxtaur2D.Logging;
using LibRenderer.Abstractions.Drawers;
using LibRenderer.Implementations.Drawers;
using LibWebClient.Services.Abstract;
using LibWebClient.Services.Implementations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;

namespace Foxtaur2D;

public class Program
{
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
        // Preparing DI
        Di = ConfigureServices()
            .BuildServiceProvider();
        
        var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", true, true);

        var configuration = builder.Build();
        
        // Setting-up NLog
        ConfigurationItemFactory
            .Default
            .Targets
            .RegisterDefinition("ControlLogging", typeof(ControlLoggingTarget));
        
        LogManager.Configuration = new NLogLoggingConfiguration(configuration.GetSection("NLog"));
        
        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    // Avalonia configuration, don't remove; also used by visual designer.
    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .LogToTrace()
            .UseReactiveUI();
    
    // Setting up DI
    public static IServiceCollection ConfigureServices()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton<IWebClientRaw, WebClientRawStub>();
        services.AddSingleton<IWebClient, WebClient>();
        services.AddSingleton<ITextDrawer, TextDrawer>();
        
        return services;
    }
    
    // Get main window
    public static Window GetMainWindow()
    {
        if (Application.Current.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
        {
            return desktopLifetime.MainWindow;
        }

        return null;
    }
}