using FoxtaurTracker.Views;

namespace FoxtaurTracker;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        // Setting up routing
        Routing.RegisterRoute("mainPage", typeof(MainPage));
        Routing.RegisterRoute("loginPage", typeof(LoginPage));
        Routing.RegisterRoute("registrationPage", typeof(RegistrationPage));
    }
}
