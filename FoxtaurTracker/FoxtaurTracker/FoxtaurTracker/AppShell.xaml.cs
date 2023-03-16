﻿using FoxtaurTracker.Views;

namespace FoxtaurTracker;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        // Setting up routing
        Routing.RegisterRoute("connectPage", typeof(ConnectPage));
        Routing.RegisterRoute("loginPage", typeof(LoginPage));
        Routing.RegisterRoute("registrationPage", typeof(RegistrationPage));
        Routing.RegisterRoute("mainPage", typeof(MainPage));
    }
}
