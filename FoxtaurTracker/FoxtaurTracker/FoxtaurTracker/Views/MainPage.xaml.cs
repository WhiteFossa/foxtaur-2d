using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoxtaurTracker.ViewModels;

namespace FoxtaurTracker.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
    }
    
    /// <summary>
    /// Page loaded handler
    /// </summary>
    private async void MainPageLoaded(object sender, EventArgs e)
    {
        await (BindingContext as MainViewModel).OnPageLoadedAsync(sender, e);
    }
    
    /// <summary>
    /// Disabling hardware back button
    /// </summary>
    protected override bool OnBackButtonPressed()
    {
        return true;
    }
}