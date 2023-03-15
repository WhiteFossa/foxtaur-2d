using FoxtaurTracker.Services.Abstract;

namespace FoxtaurTracker;

public partial class App : Application
{
	public static IServiceProvider ServicesProvider;
	public static IPopupsService PopupsService;

	public App(IServiceProvider servicesProvider)
	{
		ServicesProvider = servicesProvider;
		PopupsService = servicesProvider.GetService<IPopupsService>();
		
		InitializeComponent();

		MainPage = new AppShell();
	}
}
