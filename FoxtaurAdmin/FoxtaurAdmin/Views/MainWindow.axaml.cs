using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Input;
using FoxtaurAdmin.ViewModels;

namespace FoxtaurAdmin.Views;

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
    }
    
}