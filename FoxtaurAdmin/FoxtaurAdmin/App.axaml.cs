using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using FoxtaurAdmin.Models;
using FoxtaurAdmin.ViewModels;
using FoxtaurAdmin.Views;

namespace FoxtaurAdmin;

public partial class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow(new MainWindowViewModel(new MainModel()));
        }

        base.OnFrameworkInitializationCompleted();
    }
}