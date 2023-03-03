using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using Foxtaur2D.Controls;
using Foxtaur2D.Models;
using LibRenderer.Enums;
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

    private int _selectedHunterIndex;
    private IList<Hunter> _hunters = new List<Hunter>();

    private int _selectedTeamIndex;
    private IList<Team> _teams = new List<Team>();

    private MainModel _mainModel;

    private bool _isSingleHunterModeChecked;
    private bool _isSingleTeamModeChecked;
    private bool _isEveryoneModeChecked;

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
    /// Distances
    /// </summary>
    public IList<Distance> Distances
    {
        get => _distances;
        set
        {
            this.RaiseAndSetIfChanged(ref _distances, value);
        }
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
            
            // Updating teams list
            Teams = _mainModel.Distance != null
                ? _mainModel
                    .Distance
                    .Hunters
                    .Select(h => h.Team)
                    .Distinct()
                    .ToList()
                : new List<Team>();

            IsEveryoneModeChecked = true;
            _mainModel.HuntersFilteringMode = HuntersFilteringMode.Everyone;
            SelectedHunterIndex = -1;
            SelectedTeamIndex = -1;
            
            if (Renderer != null)
            {
                Renderer.SetActiveDistance(_mainModel.Distance);
                Renderer.SetHuntersFilteringMode(_mainModel.HuntersFilteringMode);
            }
        }
    }

    /// <summary>
    /// Selected hunter index
    /// </summary>
    public int SelectedHunterIndex
    {
        get => _selectedHunterIndex;

        set
        {
            this.RaiseAndSetIfChanged(ref _selectedHunterIndex, value);

            if (value == -1)
            {
                _mainModel.DisplayedHunter = null;
            }
            else
            {
                _mainModel.DisplayedHunter = Hunters[value];
            }

            if (Renderer != null)
            {
                Renderer.SetHunterToDisplay(_mainModel.DisplayedHunter);
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
    /// Selected team index
    /// </summary>
    public int SelectedTeamIndex
    {
        get => _selectedTeamIndex;

        set
        {
            this.RaiseAndSetIfChanged(ref _selectedTeamIndex, value);

            if (value == -1)
            {
                _mainModel.DisplayedTeam = null;
            }
            else
            {
                _mainModel.DisplayedTeam = Teams[value];
            }

            if (Renderer != null)
            {
                Renderer.SetTeamToDisplay(_mainModel.DisplayedTeam);
            }
        }
    }
    
    /// <summary>
    /// Current distance teams
    /// </summary>
    public IList<Team> Teams
    {
        get => _teams;
        set
        {
            this.RaiseAndSetIfChanged(ref _teams, value);
        }
    }

    /// <summary>
    /// Map renderer control
    /// </summary>
    public MapControl Renderer;

    /// <summary>
    /// Is single hunter mode?
    /// </summary>
    public bool IsSingleHunterModeChecked
    {
        get => _isSingleHunterModeChecked;

        set
        {
            this.RaiseAndSetIfChanged(ref _isSingleHunterModeChecked, value);

            if (value)
            {
                _mainModel.HuntersFilteringMode = HuntersFilteringMode.OneHunter;

                if (Renderer != null)
                {
                    Renderer.SetHuntersFilteringMode(_mainModel.HuntersFilteringMode);
                }
            }
        }
    }

    /// <summary>
    /// Is single team mode checked?
    /// </summary>
    public bool IsSingleTeamModeChecked
    {
        get => _isSingleTeamModeChecked;

        set
        {
            this.RaiseAndSetIfChanged(ref _isSingleTeamModeChecked, value);

            if (value)
            {
                _mainModel.HuntersFilteringMode = HuntersFilteringMode.OneTeam;
                
                if (Renderer != null)
                {
                    Renderer.SetHuntersFilteringMode(_mainModel.HuntersFilteringMode);
                }
            }
        }
    }

    /// <summary>
    /// Is "show everyone" mode checked?
    /// </summary>
    public bool IsEveryoneModeChecked
    {
        get => _isEveryoneModeChecked;

        set
        {
            this.RaiseAndSetIfChanged(ref _isEveryoneModeChecked, value);

            if (value)
            {
                _mainModel.HuntersFilteringMode = HuntersFilteringMode.Everyone;
                
                if (Renderer != null)
                {
                    Renderer.SetHuntersFilteringMode(_mainModel.HuntersFilteringMode);
                }
            }
        }
    }

    /// <summary>
    /// Focus on distance
    /// </summary>
    public ReactiveCommand<Unit, Unit> FocusOnDistanceCommand { get; }

    /// <summary>
    /// Reload distances
    /// </summary>
    public ReactiveCommand<Unit, Unit> ReloadDistancesCommand { get; set; }

    public MainWindowViewModel(MainModel mainModel)
    {
        _mainModel = mainModel ?? throw new ArgumentNullException(nameof(mainModel));
        
        // Binding commands
        var CanFocusOnDistanceCommand = this.WhenAnyValue(
            x => x.SelectedDistanceIndex,
            (selectedIndex) => selectedIndex != -1);
            
        FocusOnDistanceCommand = ReactiveCommand.Create(OnFocusOnDistanceCommand, CanFocusOnDistanceCommand);
        ReloadDistancesCommand = ReactiveCommand.Create(OnReloadDistancesCommand);

        // Asking for distances
        LoadDistances();
        
        // Showing everyone by default
        IsEveryoneModeChecked = true;
        _mainModel.HuntersFilteringMode = HuntersFilteringMode.Everyone;
        SelectedHunterIndex = -1;
        SelectedTeamIndex = -1;
    }

    /// <summary>
    /// Reload distances
    /// </summary>
    private void OnReloadDistancesCommand()
    {
        LoadDistances();
    }

    /// <summary>
    /// Load distances
    /// </summary>
    public void LoadDistances()
    {
        SelectedDistanceIndex = -1;
        
        _distances = _webClient.GetDistancesWithoutIncludeAsync()
            .Result
            .ToList();
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