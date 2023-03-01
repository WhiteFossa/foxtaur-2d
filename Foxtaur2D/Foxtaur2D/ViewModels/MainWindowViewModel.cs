using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using Foxtaur2D.Controls;
using Foxtaur2D.Models;
using LibWebClient.Models;
using LibWebClient.Services.Abstract;
using ReactiveUI;
using Microsoft.Extensions.DependencyInjection;

namespace Foxtaur2D.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private string _consoleText;
    private int _consoleCaretIndex;
    
    private int _selectedDistanceIndex;
    private IList<Distance> _distances = new List<Distance>();

    private IList<Hunter> _hunters = new List<Hunter>();

    private MainModel _mainModel;

    #region DI
    
    private readonly IWebClient _webClient = Program.Di.GetService<IWebClient>();

    #endregion
    
    /// <summary>
    /// Text in console
    /// </summary>
    public string ConsoleText
    {
        get => _consoleText;
        set => this.RaiseAndSetIfChanged(ref _consoleText, value);
    }

    /// <summary>
    /// Consone caret index (to scroll programmatically)
    /// </summary>
    public int ConsoleCaretIndex
    {
        get => _consoleCaretIndex;
        set => this.RaiseAndSetIfChanged(ref _consoleCaretIndex, value);
    }
    
    /// <summary>
    /// Selected distance index
    /// </summary>
    public int SelectedDistanceIndex
    {
        get => _selectedDistanceIndex;
        set
        {
            this.RaiseAndSetIfChanged(ref _selectedDistanceIndex, value);
                
            // Loading full distance data into model
            if (value == -1)
            {
                _mainModel.Distance = null;
            }
            else
            {
                _mainModel.Distance = _webClient.GetDistanceByIdAsync(_distances[value].Id).Result;
            }
            
            // Updating hunters list
            Hunters = _mainModel.Distance != null ? _mainModel.Distance.Hunters.ToList() : new List<Hunter>();

            if (Renderer != null)
            {
                Renderer.SetActiveDistance(_mainModel.Distance);                    
            }
        }
    }

    /// <summary>
    /// Current distance's hunters
    /// </summary>
    public IList<Hunter> Hunters
    {
        get => _hunters;
        set
        {
            this.RaiseAndSetIfChanged(ref _hunters, value);
        }
    }

    /// <summary>
    /// Map renderer control
    /// </summary>
    public MapControl Renderer;
    
    /// <summary>
    /// Focus on distance
    /// </summary>
    public ReactiveCommand<Unit, Unit> FocusOnDistanceCommand { get; }

    public MainWindowViewModel(MainModel mainModel)
    {
        _mainModel = mainModel ?? throw new ArgumentNullException(nameof(mainModel));
        
        // Binding commands
        var CanFocusOnDistanceCommand = this.WhenAnyValue(
            x => x.SelectedDistanceIndex,
            (selectedIndex) => selectedIndex != -1);
            
        FocusOnDistanceCommand = ReactiveCommand.Create(OnFocusOnDistanceCommand, CanFocusOnDistanceCommand);
        
        // Asking for distances
        SelectedDistanceIndex = -1;
        
    }
    
    /// <summary>
    /// Return distances list
    /// </summary>
    public IList<Distance> GetDistances()
    {
        _distances = _webClient.GetDistancesWithoutIncludeAsync()
            .Result
            .ToList();
        
        return _distances;
    }

    /// <summary>
    /// Focus on distance
    /// </summary>
    private void OnFocusOnDistanceCommand()
    {
        Renderer.FocusOnDistance();
    }
    
    #region Logging

    /// <summary>
    /// Adds a new text line to console. Feed it to logger
    /// </summary>
    public void AddLineToConsole(string line)
    {
        ConsoleText += $"{line}{Environment.NewLine}";

        ConsoleCaretIndex = ConsoleText.Length;
    }

    #endregion
}