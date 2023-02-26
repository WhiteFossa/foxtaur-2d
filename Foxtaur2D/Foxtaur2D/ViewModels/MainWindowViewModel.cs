using System.Reactive;
using ReactiveUI;

namespace Foxtaur2D.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private int _selectedDistanceIndex;
    
    #region DI
    
    //private readonly IWebClient _webClient = Program.Di.GetService<IWebClient>();

    #endregion
    
    /// <summary>
    /// Focus on distance
    /// </summary>
    public ReactiveCommand<Unit, Unit> FocusOnDistanceCommand { get; }
    
}