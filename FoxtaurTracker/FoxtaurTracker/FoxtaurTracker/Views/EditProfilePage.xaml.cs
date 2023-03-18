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
}