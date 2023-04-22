using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoxtaurTracker.ViewModels;

namespace FoxtaurTracker.Views;

public partial class ManageTrackersPage : ContentPage
{
    public ManageTrackersPage()
    {
        InitializeComponent();
    }
    
    /// <summary>
    /// Page loaded handler
    /// </summary>
    private async void ManageTrackersPageLoaded(object sender, EventArgs e)
    {
        await (BindingContext as ManageTrackersViewModel).OnPageLoadedAsync(sender, e);
    }
}