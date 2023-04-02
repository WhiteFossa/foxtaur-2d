using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorPicker.Maui;
using FoxtaurTracker.ViewModels;

namespace FoxtaurTracker.Views;

public partial class EditProfilePage : ContentPage
{
    public EditProfilePage()
    {
        InitializeComponent();
    }
    
    /// <summary>
    /// Dirty hack to process hunter color changes
    /// </summary>
    public void HunterColorChanged(object sender, PickedColorChangedEventArgs args)
    {
        (BindingContext as EditProfileViewModel).HunterColor = args.NewPickedColorValue;
    }

    /// <summary>
    /// Dirty hack to set hunter color
    /// </summary>
    public void SetHunterColor(Color color)
    {
        HunterColorPicker.PickedColor = color;
    }

    /// <summary>
    /// Page loaded handler
    /// </summary>
    private async void EditProfilePageLoaded(object sender, EventArgs e)
    {
        (BindingContext as EditProfileViewModel).SetHunterColor += SetHunterColor;
        await (BindingContext as EditProfileViewModel).OnPageLoadedAsync(sender, e);
    }
}