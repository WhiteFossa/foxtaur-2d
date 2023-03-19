using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoxtaurTracker.ViewModels;

namespace FoxtaurTracker.Views;

public partial class RegisterOnDistancePage : ContentPage
{
    public RegisterOnDistancePage()
    {
        InitializeComponent();
    }
    
    /// <summary>
    /// Page loaded handler
    /// </summary>
    private async void RegisterOnDistancePageLoaded(object sender, EventArgs e)
    {
        await (BindingContext as RegisterOnDistanceViewModel).OnPageLoadedAsync(sender, e);
    }
}