using Avalonia.Controls;
using Foxtaur2D.ViewModels;

namespace Foxtaur2D.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    public MainWindow(MainWindowViewModel dataContext)
    {
        Opened += OnWindowOpened;
        
        InitializeComponent();

        DataContext = dataContext;
        
        ((MainWindowViewModel)DataContext).Renderer = MapRenderer;
    }
    
    /// <summary>
    /// Called when window opened
    /// </summary>
    private void OnWindowOpened(object sender, System.EventArgs e)
    {
        ((MainWindowViewModel)DataContext).OnWindowOpened(sender, e);
    }
}