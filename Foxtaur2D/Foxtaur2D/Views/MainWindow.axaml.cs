using System.Linq;
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
        InitializeComponent();

        DataContext = dataContext;
            
        var distancesComboBox = this.Find<ComboBox>("Distances");
        distancesComboBox.Items = ((MainWindowViewModel)DataContext)
            .GetDistances()
            .Select(d => d.Name);

        ((MainWindowViewModel)DataContext).Renderer = MapRenderer;
    }
}