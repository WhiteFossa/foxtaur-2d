using FoxtaurTracker.Services.Abstract;
using FoxtaurTracker.Services.Implementations;
using LibWebClient.Services.Abstract;
using LibWebClient.Services.Implementations;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace FoxtaurTracker;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		#region DI

		// Singletons
		builder.Services.AddSingleton<IPopupsService, PopupsService>();
		builder.Services.AddSingleton<IWebClientRaw, WebClientRaw>();
		builder.Services.AddSingleton<IWebClient, WebClient>();
		builder.Services.AddSingleton<ILocationsProcessingService, LocationsProcessingService>();
		builder.Services.AddSingleton<INotificationsService, NotificationsService>();
		builder.Services.AddSingleton<ISettingsService, SettingsService>();
		builder.Services.AddSingleton<ILoginService, LoginService>();

		#endregion

		// For Color Picker
		builder.UseSkiaSharp();

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
