using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Foxtaur2D.Models;
using Foxtaur2D.ViewModels;
using Foxtaur2D.Views;

namespace Foxtaur2D;

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