using Avalonia;
using Avalonia.ReactiveUI;
using System;
using LibRenderer.Abstractions.Drawers;
using LibRenderer.Implementations.Drawers;
using Microsoft.Extensions.DependencyInjection;

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
        
        services.AddSingleton<ITextDrawer, TextDrawer>();
        
        return services;
    }
}