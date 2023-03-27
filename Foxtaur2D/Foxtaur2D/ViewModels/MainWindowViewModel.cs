﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive;
using System.Timers;
using Avalonia.Media;
using Avalonia.Threading;
using Foxtaur2D.Controls;
using Foxtaur2D.Models;
using LibRenderer.Constants;
using LibRenderer.Enums;
using LibWebClient.Models;
using LibWebClient.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using ReactiveUI;

namespace Foxtaur2D.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
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

    private IBrush _huntersDataBackground;

    private double _huntersDataReloadInterval;
    private string _huntersDataReloadIntervalText;

    private bool _isTimelineEnabled;
    private double _timelineValueRaw;
    private double _timelineEndValueRaw;

    private string _timelineBeginTimeText;
    private string _timelineCurrentTimeText;
    private string _timelineEndTimeText;

    private DateTime _timelineBeginTime;
    private DateTime _timelineCurrentTime;
    private DateTime _timelineEndTime;

    private bool _isRealtimeUpdateMode;

    /// <summary>
    /// Timer to move end side of timeline
    /// </summary>
    private Timer _timelineUpdateTimer;

    #region DI
    
    private readonly IWebClient _webClient = Program.Di.GetService<IWebClient>();

    #endregion
    
    #region Logger
    
    /// <summary>
    /// Logger
    /// </summary>
    private Logger _logger = LogManager.GetCurrentClassLogger();
    
    #endregion

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
            
            ProcessTimelineTimes();

            if (Renderer != null)
            {
                Renderer.SetActiveDistance(_mainModel.Distance);
                Renderer.SetHuntersFilteringMode(_mainModel.HuntersFilteringMode);
                Renderer.SetHuntersDataStateInfo = SetHuntersDataState;
                Renderer.SetHuntersDataReloadInterval(HuntersDataReloadInterval);
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
    /// Background for Hunters Data indicator
    /// </summary>
    public IBrush HuntersDataBackground
    {
        get => _huntersDataBackground;

        set
        {
            this.RaiseAndSetIfChanged(ref _huntersDataBackground, value);
        }
    }

    /// <summary>
    /// Hunters data reload interval in milliseconds
    /// </summary>
    public double HuntersDataReloadInterval
    {
        get => _huntersDataReloadInterval;

        set
        {
            this.RaiseAndSetIfChanged(ref _huntersDataReloadInterval, value);
            FormatHuntersDataReloadIntervalText();

            if (Renderer != null)
            {
                Renderer.SetHuntersDataReloadInterval(value);
            }
        }
    }

    /// <summary>
    /// Hunters data reload interval text (in seconds)
    /// </summary>
    public string HuntersDataReloadIntervalText
    {
        get => _huntersDataReloadIntervalText;

        set => this.RaiseAndSetIfChanged(ref _huntersDataReloadIntervalText, value);
    }

    /// <summary>
    /// Is timeline enabled?
    /// </summary>
    public bool IsTimelineEnabled
    {
        get => _isTimelineEnabled;

        set => this.RaiseAndSetIfChanged(ref _isTimelineEnabled, value);
    }
    
    /// <summary>
    /// Timeline raw value (i.e. seconds since first hunter start)
    /// </summary>
    public double TimelineValueRaw
    {
        get => _timelineValueRaw;

        set
        {
            this.RaiseAndSetIfChanged(ref _timelineValueRaw, value);
            
            _isRealtimeUpdateMode = _timelineValueRaw >= _timelineEndValueRaw - 1.0; // If value in the last second we need to enable realtime mode

            _timelineCurrentTime = _timelineBeginTime.AddSeconds(value);
            TimelineCurrentTimeText = _timelineCurrentTime.ToLocalTime().ToLongTimeString();

            if (Renderer != null)
            {
                Renderer.SetHuntersLocationsInterval(_timelineBeginTime, _timelineCurrentTime);
            }
        }
    }

    /// <summary>
    /// Timeline end raw value
    /// </summary>
    public double TimelineEndValueRaw
    {
        get => _timelineEndValueRaw;

        set => this.RaiseAndSetIfChanged(ref _timelineEndValueRaw, value);
    }

    /// <summary>
    /// Timeline begin time as text
    /// </summary>
    public string TimelineBeginTimeText
    {
        get => _timelineBeginTimeText;

        set => this.RaiseAndSetIfChanged(ref _timelineBeginTimeText, value);
    }
    
    /// <summary>
    /// Timeline current time as text
    /// </summary>
    public string TimelineCurrentTimeText
    {
        get => _timelineCurrentTimeText;

        set => this.RaiseAndSetIfChanged(ref _timelineCurrentTimeText, value);
    }
    
    /// <summary>
    /// Timeline end time as text
    /// </summary>
    public string TimelineEndTimeText
    {
        get => _timelineEndTimeText;

        set
        {
            _logger.Info($"End time: {value}");
            this.RaiseAndSetIfChanged(ref _timelineEndTimeText, value);
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
        _isRealtimeUpdateMode = true;
        ProcessTimelineTimes();
        
        // Marking initial data state
        SetHuntersDataState(HuntersDataState.Downloaded);
        
        // Initial interval
        HuntersDataReloadInterval = 1000; // TODO: Save/load it
        
        // Timeline update timer
        _timelineUpdateTimer = new Timer(1000); // No need to move into constants, magical 1 second
        _timelineUpdateTimer.Elapsed += OnTimelineUpdateTimer;
        _timelineUpdateTimer.AutoReset = true;
        _timelineUpdateTimer.Enabled = true;
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

    /// <summary>
    /// Set hunters data state
    /// </summary>
    public void SetHuntersDataState(HuntersDataState state)
    {
        switch (state)
        {
            case HuntersDataState.Failed:
                HuntersDataBackground = new SolidColorBrush(RendererConstants.HuntersDataDownloadFailureColor);
                return;
                
            case HuntersDataState.DownloadInitiated:
                HuntersDataBackground = new SolidColorBrush(RendererConstants.HuntersDataDownloadInitiatedColor);
                return;
            
            case HuntersDataState.Downloaded:
                HuntersDataBackground = new SolidColorBrush(RendererConstants.HuntersDataDownloadCompletedColor);
                return;
            
            default:
                throw new ArgumentException("Unknown hunters data state");
        }
    }

    private void FormatHuntersDataReloadIntervalText()
    {
        HuntersDataReloadIntervalText = $"{(HuntersDataReloadInterval/1000.0):.0}s";
    }

    private void ProcessTimelineTimes()
    {
        if (_mainModel.Distance == null)
        {
            IsTimelineEnabled = false;
            TimelineValueRaw = 0;
            TimelineEndValueRaw = 0;
            TimelineBeginTimeText = "N/A";
            TimelineCurrentTimeText = "N/A";
            TimelineEndTimeText = "N/A";
            return;
        }

        _timelineBeginTime = _mainModel.Distance.FirstHunterStartTime;
        
        var currentTime = DateTime.UtcNow;
        if (currentTime < _mainModel.Distance.FirstHunterStartTime)
        {
            _timelineCurrentTime = _mainModel.Distance.FirstHunterStartTime;
            _timelineEndTime = _mainModel.Distance.CloseTime;
        }
        else
        {
            if (currentTime <= _mainModel.Distance.CloseTime)
            {
                _timelineCurrentTime = currentTime;
                _timelineEndTime = currentTime;
            }
            else
            {
                _timelineCurrentTime = _mainModel.Distance.CloseTime;
                _timelineEndTime = _mainModel.Distance.CloseTime;
            }
        }
        
        ProcessRealtimeMode();
        
        IsTimelineEnabled = true;
        TimelineBeginTimeText = _timelineBeginTime.ToLocalTime().ToLongTimeString();
        TimelineCurrentTimeText = _timelineCurrentTime.ToLocalTime().ToLongTimeString();
        TimelineEndTimeText = _timelineEndTime.ToLocalTime().ToLongTimeString();
        TimelineEndValueRaw = (_timelineEndTime - _timelineBeginTime).TotalSeconds;
        TimelineValueRaw = (_timelineCurrentTime - _timelineBeginTime).TotalSeconds;
        
        if (Renderer != null)
        {
            Renderer.SetHuntersLocationsInterval(_timelineBeginTime, _timelineCurrentTime);
        }
    }

    private void ProcessRealtimeMode()
    {
        var currentTime = DateTime.UtcNow;
        if (_mainModel.Distance == null
            || currentTime < _mainModel.Distance.FirstHunterStartTime
            || currentTime > _mainModel.Distance.CloseTime)
        {
            return;
        }

        // If we are in realtime update mode we need to force currentTime = endTime
        if (_isRealtimeUpdateMode)
        {
            _timelineEndTime = DateTime.UtcNow;
            _timelineCurrentTime = _timelineEndTime;
            
            TimelineCurrentTimeText = _timelineCurrentTime.ToLocalTime().ToLongTimeString();
            TimelineEndTimeText = _timelineEndTime.ToLocalTime().ToLongTimeString();

            TimelineEndValueRaw = (_timelineEndTime - _timelineBeginTime).TotalSeconds;
            TimelineValueRaw = (_timelineCurrentTime - _timelineBeginTime).TotalSeconds;
        
            if (Renderer != null)
            {
                Renderer.SetHuntersLocationsInterval(_timelineBeginTime, _timelineCurrentTime);
            }
        }
    }
    
    private void OnTimelineUpdateTimer(object sender, ElapsedEventArgs e)
    {
        var currentTime = DateTime.UtcNow;
        if (_mainModel.Distance != null
            && currentTime >= _mainModel.Distance.FirstHunterStartTime
            && currentTime <= _mainModel.Distance.CloseTime)
        {
            _timelineEndTime = currentTime;
        
            TimelineEndValueRaw = (_timelineEndTime - _timelineBeginTime).TotalSeconds;
            TimelineEndTimeText = _timelineEndTime.ToLocalTime().ToLongTimeString();

            ProcessRealtimeMode();
        }
    }
}