using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FoxtaurTracker.ViewModels;

namespace FoxtaurTracker.Views;

public partial class RunPage : ContentPage
{
    public RunPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Page loaded handler
    /// </summary>
    private async void RunPageLoaded(object sender, EventArgs e)
    {
        await (BindingContext as RunViewModel).OnPageLoadedAsync(sender, e);
    }
}