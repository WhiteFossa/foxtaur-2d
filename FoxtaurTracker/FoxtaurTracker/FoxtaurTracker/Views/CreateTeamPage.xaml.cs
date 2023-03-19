﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorPicker.Maui;
using FoxtaurTracker.ViewModels;

namespace FoxtaurTracker.Views;

public partial class CreateTeamPage : ContentPage
{
    public CreateTeamPage()
    {
        InitializeComponent();
    }

    /// <summary>
    /// Dirty hack to process team color changes
    /// </summary>
    public void TeamColorChanged(object sender, PickedColorChangedEventArgs args)
    {
        (BindingContext as CreateTeamViewModel).Color = args.NewPickedColorValue;
    }
}