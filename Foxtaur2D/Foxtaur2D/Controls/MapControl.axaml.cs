using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;
using LibBusinessLogic.Services.Abstract;
using LibGeo.Abstractions;
using LibGeo.Implementations;
using LibGeo.Models;
using LibGpsFilter.Abstractions;
using LibRenderer.Abstractions.Drawers;
using LibRenderer.Abstractions.Layers;
using LibRenderer.Constants;
using LibRenderer.Enums;
using LibRenderer.Implementations.Layers;
using LibRenderer.Implementations.UI;
using LibWebClient.Models;
using LibWebClient.Models.Requests;
using LibWebClient.Services.Abstract;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using Timer = System.Timers.Timer;

namespace Foxtaur2D.Controls;

#region Delegates

/// <summary>
/// Delegate for setting information about current hunters data state
/// </summary>
public delegate void SetHunterDataStateInfoDelegate(HuntersDataState state);

#endregion

public partial class MapControl : UserControl
{
    #region Constants

    #region Layers order

    /// <summary>
    /// Basemap layer order
    /// </summary>
    private const int BasemapLayerOrder = 0;

    /// <summary>
    /// Distance layer order
    /// </summary>
    private const int DistanceLayerOrder = 1000;

    /// <summary>
    /// Hunters layer order
    /// </summary>
    private const int HuntersLayerOrder = 2000;

    /// <summary>
    /// Scale layer order
    /// </summary>
    private const int ScaleRulerLayerOrder = 3000;

    #endregion

    #endregion

    #region Control sizes

    /// <summary>
    /// Screen scaling
    /// </summary>
    private double _scaling;

    /// <summary>
    /// Viewport width
    /// </summary>
    private int _viewportWidth;

    /// <summary>
    /// Viewport height
    /// </summary>
    private int _viewportHeight;

    #endregion

    #region Drawing

    /// <summary>
    /// Backing image array
    /// </summary>
    private Byte[] _backingArray;

    /// <summary>
    /// Layers to display
    /// </summary>
    private List<ILayer> _layers = new List<ILayer>();

    /// <summary>
    /// Backing image geoprovider
    /// </summary>
    private IGeoProvider _backingImageGeoProvider;

    /// <summary>
    /// UI drawer
    /// </summary>
    private UiDrawer _uiDrawer = new UiDrawer();

    /// <summary>
    /// Processed image, ready to be displayed
    /// </summary>
    private Bitmap _displayBitmap;

    #endregion

    #region Mouse

    private bool _isDisplayMoving;

    private double _oldMouseX;
    private double _oldMouseY;

    #endregion

    #region Distances

    private Distance _activeDistance;

    /// <summary>
    /// Distance layer
    /// </summary>
    private DistanceLayer _distanceLayer;

    #endregion

    #region Hunters

    /// <summary>
    /// One hunter to display (for case when only one hunter have to be displayed)
    /// </summary>
    private Hunter _hunterToDisplay;

    /// <summary>
    /// One team to display (for case when only one team have to be displayed)
    /// </summary>
    private Team _teamToDisplay;

    /// <summary>
    /// Hunters filtering/display mode
    /// </summary>
    private HuntersFilteringMode _huntersFilteringMode;

    /// <summary>
    /// Hunters after preliminary filtering (team / individual, GPS denoising)
    /// </summary>
    private IReadOnlyCollection<Hunter> _filteredHunters;

    /// <summary>
    /// Hunters, filtered by begin and end time with processed GPS tracks and so on
    /// </summary>
    private IReadOnlyCollection<Hunter> _huntersToDisplay;

    #endregion

    /// <summary>
    /// Logger
    /// </summary>
    private Logger _logger = LogManager.GetCurrentClassLogger();

    #region DI

    private readonly ITextDrawer _textDrawer;
    private readonly IWebClient _webClient;
    private readonly IGpsFilter _gpsFilter;
    private readonly ITeamsService _teamsService;

    #endregion

    #region Reloading

    /// <summary>
    /// Hunters data is reloaded on this timer tick
    /// </summary>
    private Timer _huntersDataReloadTimer;

    /// <summary>
    /// Mutex to protect hunters data from corruption
    /// </summary>
    private Mutex _huntersDataReloadMutex = new Mutex();

    /// <summary>
    /// Call this to display hunters data state info on UI
    /// </summary>
    public SetHunterDataStateInfoDelegate SetHuntersDataStateInfo;

    #endregion

    #region Hunters histories interval

    private DateTime _huntersHistoriesBeginTime;

    private DateTime _huntersHistoriesEndTime;

    #endregion

    #region Redrawing

    /// <summary>
    /// Mutex to protect from multiple InvalidateVisual() on different threads
    /// </summary>
    private Mutex _invalidateVisualMutex = new Mutex();

    #endregion

    #region Maps processing

    /// <summary>
    /// Call this to display map progress state
    /// </summary>
    public SetMapProgressStateDelegate SetMapProgressState;

    #endregion

    #region Debug

    #endregion

    public MapControl()
    {
        _textDrawer = Program.Di.GetService<ITextDrawer>();
        _webClient = Program.Di.GetService<IWebClient>();
        _gpsFilter = Program.Di.GetService<IGpsFilter>();
        _teamsService = Program.Di.GetService<ITeamsService>();

        InitializeComponent();

        _backingArray = null; // It will remain null till the first resize

        // Layers creation
        _layers.Add(new FlatImageLayer(@"Resources/HYP_50M_SR_W.jpeg", BasemapLayerOrder));
        _layers.Add(new HuntersLayer(HuntersLayerOrder));
        _layers.Add(new ScaleRulerLayer(ScaleRulerLayerOrder));

        OrderLayers();

        // Listening for properties changes to process resize
        PropertyChanged += OnPropertyChangedListener;

        // Setting-up input events
        PointerPressed += OnMousePressed;
        PointerReleased += OnMouseReleased;
        PointerMoved += OnMouseMoved;
        PointerWheelChanged += OnWheel;

        // Setting up reload timer
        _huntersDataReloadTimer = new Timer(1000); // TODO: Set me in UI
        _huntersDataReloadTimer.Elapsed += OnHuntersDataReloadTimer;
        _huntersDataReloadTimer.AutoReset = true;
        _huntersDataReloadTimer.Enabled = true;
    }

    private void OnMousePressed(object sender, PointerPressedEventArgs e)
    {
        _oldMouseX = e.GetCurrentPoint(this).Position.X * _scaling;
        _oldMouseY = e.GetCurrentPoint(this).Position.Y * _scaling;

        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            _isDisplayMoving = true;
        }
    }

    private void OnMouseMoved(object sender, PointerEventArgs e)
    {
        var newMouseX = e.GetCurrentPoint(this).Position.X * _scaling;
        var newMouseY = e.GetCurrentPoint(this).Position.Y * _scaling;

        if (_isDisplayMoving)
        {
            (_backingImageGeoProvider as DisplayGeoProvider).MoveDisplay(_oldMouseX, _oldMouseY, newMouseX, newMouseY);

            _displayBitmap = null;
        }

        _uiDrawer.Data.MouseLat = _backingImageGeoProvider.YToLat(newMouseY);
        _uiDrawer.Data.MouseLon = _backingImageGeoProvider.XToLon(newMouseX);

        _oldMouseX = newMouseX;
        _oldMouseY = newMouseY;

        SynchronizedInvalidateVisual();
    }

    private void OnMouseReleased(object sender, PointerReleasedEventArgs e)
    {
        if (!e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            _isDisplayMoving = false;
        }
    }

    private void OnWheel(object sender, PointerWheelEventArgs e)
    {
        var steps = Math.Abs(e.Delta.Y);

        double zoomFactor;
        if (e.Delta.Y > 0)
        {
            zoomFactor = RendererConstants.ZoomInStep;
        }
        else
        {
            zoomFactor = RendererConstants.ZoomOutStep;
        }

        (_backingImageGeoProvider as DisplayGeoProvider).Zoom(
            (_backingImageGeoProvider as DisplayGeoProvider).PixelSize * zoomFactor, _oldMouseX, _oldMouseY);

        _displayBitmap = null;

        SynchronizedInvalidateVisual();
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    /// <summary>
    /// Properties change listener
    /// </summary>
    private void OnPropertyChangedListener(object sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property.Name.Equals("Bounds"))
        {
            // Resize event
            OnResize((Rect)e.NewValue);
        }
    }

    /// <summary>
    /// Called when control resized
    /// </summary>
    private void OnResize(Rect bounds)
    {
        _scaling = VisualRoot.RenderScaling;

        _viewportWidth = (int)(bounds.Width * _scaling);
        _viewportHeight = (int)(bounds.Height * _scaling);

        // Recreating backing array
        _backingArray = new byte[_viewportWidth * _viewportHeight * 4];

        // Re-setup backing image geoprovider
        if (_backingImageGeoProvider == null)
        {
            // First resize
            _backingImageGeoProvider = new DisplayGeoProvider(_viewportWidth, _viewportHeight);
        }
        else
        {
            (_backingImageGeoProvider as DisplayGeoProvider).OnResize(_viewportWidth, _viewportHeight);
        }


        _displayBitmap = null;
    }

    /// <summary>
    /// Render the control
    /// </summary>
    public override void Render(DrawingContext context)
    {
        base.Render(context);

        // Regenerating background image if needed
        if (_displayBitmap == null)
        {
            GenerateDisplayBitmap();
        }

        context.DrawImage(_displayBitmap, new Rect(0, 0, _viewportWidth, _viewportHeight));

        // Vector layers
        DrawVectorLayers(context);

        // UI
        _uiDrawer.Draw(context, _viewportWidth, _viewportHeight, _scaling);
    }

    /// <summary>
    /// Draw raster layers (to a bitmap)
    /// </summary>
    private unsafe void GenerateDisplayBitmap()
    {
        foreach (var layer in _layers)
        {
            if (layer is IRasterLayer)
            {
                // Raster layer
                var rasterLayer = layer as IRasterLayer;

                if (!rasterLayer.IsReady())
                {
                    // Is not ready yet
                    continue;
                }

                var layerPixels = rasterLayer.GetPixelsArray();
                for (var y = 0; y < _viewportHeight; y++)
                {
                    Parallel.For(0, _viewportWidth,
                        x =>
                        {
                            var backingLat = _backingImageGeoProvider.YToLat(y);
                            var backingLon = _backingImageGeoProvider.XToLon(x);
                            var backingIndex = (y * _viewportWidth + x) * 4;

                            var isPixelExist = rasterLayer.GetPixelCoordinates(backingLat, backingLon, out var layerX,
                                out var layerY);
                            if (isPixelExist)
                            {
                                GetPixelWithInterpolation(layerPixels, rasterLayer.Width, rasterLayer.Height,
                                    (int)layerX, (int)layerY, out var lp0, out var lp1, out var lp2, out var lp3);

                                var opacity = lp3 / (double)0xFF;

                                _backingArray[backingIndex] = MixBrightness(lp0, _backingArray[backingIndex], opacity);
                                _backingArray[backingIndex + 1] =
                                    MixBrightness(lp1, _backingArray[backingIndex + 1], opacity);
                                _backingArray[backingIndex + 2] =
                                    MixBrightness(lp2, _backingArray[backingIndex + 2], opacity);
                                _backingArray[backingIndex + 3] = 0xFF;
                            }
                        });
                }
            }
        }

        // Rendering backing image
        fixed (byte* pixels = _backingArray)
        {
            _displayBitmap = new Bitmap(
                PixelFormat.Rgba8888,
                AlphaFormat.Opaque,
                (nint)pixels,
                new PixelSize(_viewportWidth, _viewportHeight),
                new Vector(RendererConstants.DefaultDPI / _scaling, RendererConstants.DefaultDPI / _scaling),
                _viewportWidth * 4);
        }
    }

    /// <summary>
    /// Draw vector layers
    /// </summary>
    private void DrawVectorLayers(DrawingContext context)
    {
        foreach (var layer in _layers)
        {
            if (layer is IVectorLayer)
            {
                var vectorLayer = layer as IVectorLayer;

                if (layer is IHuntersVectorLayer)
                {
                    // Special case - hunters layer
                    (vectorLayer as IHuntersVectorLayer).Draw(context, _viewportWidth, _viewportHeight, _scaling,
                        _backingImageGeoProvider, _huntersToDisplay);
                }
                else
                {
                    vectorLayer.Draw(context, _viewportWidth, _viewportHeight, _scaling, _backingImageGeoProvider);
                }
            }
        }
    }

    public void GetPixelWithInterpolation(byte[] pixels, int width, int height, double x, double y, out byte r0,
        out byte r1, out byte r2, out byte r3)
    {
        var x1 = (int)x;
        var y1 = (int)y;

        if (x1 == width - 1 || y1 == height - 1)
        {
            // Edge pixel
            GetPixel(pixels, width, x1, y1, out r0, out r1, out r2, out r3);
            return;
        }

        var x2 = x1 + 1;
        var y2 = y1 + 1;

        GetPixel(pixels, width, x1, y1, out var p10, out var p11, out var p12, out var p13);
        GetPixel(pixels, width, x2, y1, out var p20, out var p21, out var p22, out var p23);
        GetPixel(pixels, width, x2, y2, out var p30, out var p31, out var p32, out var p33);
        GetPixel(pixels, width, x1, y2, out var p40, out var p41, out var p42, out var p43);

        // y2 - y1 is always 1
        var k1 = y - y1;
        var k2 = y2 - y;

        var q10 = k1 * p40 + k2 * p10;
        var q11 = k1 * p41 + k2 * p11;
        var q12 = k1 * p42 + k2 * p12;
        var q13 = k1 * p43 + k2 * p13;

        var q20 = k1 * p30 + k2 * p20;
        var q21 = k1 * p31 + k2 * p21;
        var q22 = k1 * p32 + k2 * p22;
        var q23 = k1 * p33 + k2 * p23;

        // x2 - x1 is always 1
        var k3 = x - x1;
        var k4 = x2 - x;

        r0 = (byte)Math.Floor(q10 * k4 + q20 * k3 + 0.5);
        r1 = (byte)Math.Floor(q11 * k4 + q21 * k3 + 0.5);
        r2 = (byte)Math.Floor(q12 * k4 + q22 * k3 + 0.5);
        r3 = (byte)Math.Floor(q13 * k4 + q23 * k3 + 0.5);
    }

    /// <summary>
    /// To avoid heap allocation
    /// </summary>
    private void GetPixel(byte[] pixels, int width, int x, int y, out byte r0, out byte r1, out byte r2, out byte r3)
    {
        var index = (y * width + x) * 4;

        r0 = pixels[index + 0];
        r1 = pixels[index + 1];
        r2 = pixels[index + 2];
        r3 = pixels[index + 3];
    }

    private byte MixBrightness(byte topBrightness, byte bottomBrightness, double multiplier)
    {
        int newBrightness = (int)Math.Floor(topBrightness * multiplier + bottomBrightness * (1 - multiplier) + 0.5);
        if (newBrightness > 0xFF)
        {
            newBrightness = 0xFF;
        }
        else if (newBrightness < 0)
        {
            newBrightness = 0;
        }

        return (byte)newBrightness;
    }

    /// <summary>
    /// Set active distance
    /// </summary>
    public void SetActiveDistance(Distance distance)
    {
        _activeDistance = distance;

        _layers.Remove(_distanceLayer);
        
        // Stopping old distance layer activities
        if (_distanceLayer != null)
        {
            _distanceLayer.Cleanup();
        }
        
        if (_activeDistance != null)
        {
            AddDistanceLayer();

            _huntersHistoriesBeginTime = _activeDistance.FirstHunterStartTime;
            _huntersHistoriesEndTime = _activeDistance.CloseTime;

            _filteredHunters = ApplyHuntersFilter(_activeDistance.Hunters);

            var filteredByInterval = FilterHuntersLocationsByHistoriesInterval(_filteredHunters);
            _huntersToDisplay = GpsFilterHuntersLocations(filteredByInterval);
        }

        _displayBitmap = null;

        SynchronizedInvalidateVisual();
    }

    /// <summary>
    /// Focus on current active distance
    /// </summary>
    public void FocusOnDistance()
    {
        if (_activeDistance == null)
        {
            return;
        }

        var latCenter = (_activeDistance.Map.NorthLat + _activeDistance.Map.SouthLat) / 2.0;
        var lonCenter = (_activeDistance.Map.EastLon + _activeDistance.Map.WestLon) / 2.0;

        var geoProvider = (_backingImageGeoProvider as DisplayGeoProvider);

        geoProvider.ZoomTo(
            new GeoPoint(_activeDistance.Map.NorthLat, _activeDistance.Map.WestLon),
            new GeoPoint(_activeDistance.Map.SouthLat, _activeDistance.Map.EastLon));

        geoProvider.CenterDisplay(latCenter, lonCenter);

        _displayBitmap = null;

        SynchronizedInvalidateVisual();
    }

    /// <summary>
    /// Called when distance loaded
    /// </summary>
    private void OnDistanceLoadedHandler()
    {
        Dispatcher.UIThread.InvokeAsync(() =>
        {
            // Distance have raster component, so invalidate raster image and redraw
            _displayBitmap = null;

            SynchronizedInvalidateVisual();
        });
    }

    /// <summary>
    /// Set hunter to display (for "display one hunter" mode)
    /// </summary>
    public void SetHunterToDisplay(Hunter hunter)
    {
        _hunterToDisplay = hunter;

        if (_huntersFilteringMode == HuntersFilteringMode.OneHunter)
        {
            _filteredHunters = ApplyHuntersFilter(_activeDistance.Hunters);

            var filteredByInterval = FilterHuntersLocationsByHistoriesInterval(_filteredHunters);
            _huntersToDisplay = GpsFilterHuntersLocations(filteredByInterval);

            SynchronizedInvalidateVisual();
        }
    }

    /// <summary>
    /// Set team to display (for "display one team" mode)
    /// </summary>
    public void SetTeamToDisplay(Team team)
    {
        _teamToDisplay = team;

        if (_huntersFilteringMode == HuntersFilteringMode.OneTeam)
        {
            _filteredHunters = ApplyHuntersFilter(_activeDistance.Hunters);

            var filteredByInterval = FilterHuntersLocationsByHistoriesInterval(_filteredHunters);
            _huntersToDisplay = GpsFilterHuntersLocations(filteredByInterval);

            SynchronizedInvalidateVisual();
        }
    }

    /// <summary>
    /// Set hunters filtering mode
    /// </summary>
    public void SetHuntersFilteringMode(HuntersFilteringMode filteringMode)
    {
        _huntersFilteringMode = filteringMode;

        _filteredHunters = ApplyHuntersFilter(_activeDistance?.Hunters);

        var filteredByInterval = FilterHuntersLocationsByHistoriesInterval(_filteredHunters);
        _huntersToDisplay = GpsFilterHuntersLocations(filteredByInterval);

        SynchronizedInvalidateVisual();
    }

    private IReadOnlyCollection<Hunter> ApplyHuntersFilter(IReadOnlyCollection<Hunter> hunters)
    {
        if (hunters == null)
        {
            return new List<Hunter>();
        }

        switch (_huntersFilteringMode)
        {
            case HuntersFilteringMode.OneHunter:
                if (_hunterToDisplay == null)
                {
                    return new List<Hunter>();
                }
                else
                {
                    return hunters
                        .Where(h => h.Id == _hunterToDisplay.Id)
                        .ToList();
                }

            case HuntersFilteringMode.OneTeam:
                if (_teamToDisplay == null)
                {
                    return new List<Hunter>();
                }
                else
                {
                    return hunters
                        .Where(h => h.Team != null)
                        .Where(h => h.Team.Id == _teamToDisplay.Id)
                        .ToList();
                }

            case HuntersFilteringMode.Everyone:
                if (_activeDistance == null)
                {
                    return new List<Hunter>();
                }
                else
                {
                    return hunters
                        .ToList();
                }

            default:
                throw new InvalidOperationException("Unknown hunters filtering mode!");
        }
    }

    /// <summary>
    /// Called when hunters information needs to be reloaded
    /// </summary>
    private void OnHuntersDataReloadTimer(object sender, ElapsedEventArgs e)
    {
        Dispatcher.UIThread.InvokeAsync(() => { MarkHuntersDataReloadStart(); });

        try
        {
            _huntersDataReloadMutex.WaitOne();

            if (_activeDistance == null)
            {
                Dispatcher.UIThread.InvokeAsync(() => { MarkHuntersDataAsActual(); });
                return;
            }

            var reloadDataThread = new Thread(() => ReloadHuntersData());
            reloadDataThread.Start();
        }
        finally
        {
            _huntersDataReloadMutex.ReleaseMutex();
        }
    }

    /// <summary>
    /// Marks (on UI) hunters data as actual
    /// </summary>
    private void MarkHuntersDataAsActual()
    {
        if (SetHuntersDataStateInfo != null)
        {
            SetHuntersDataStateInfo(HuntersDataState.Downloaded);
        }
    }

    /// <summary>
    /// Marks (on UI) hunters data reload start
    /// </summary>
    private void MarkHuntersDataReloadStart()
    {
        if (SetHuntersDataStateInfo != null)
        {
            SetHuntersDataStateInfo(HuntersDataState.DownloadInitiated);
        }
    }

    /// <summary>
    /// Marks (on UI) hunters data reload failure
    /// </summary>
    private void MarkHuntersDataReloadFailure()
    {
        if (SetHuntersDataStateInfo != null)
        {
            SetHuntersDataStateInfo(HuntersDataState.Failed);
        }
    }

    /// <summary>
    /// Reload hunters data. Must be called on separate thread
    /// </summary>
    private void ReloadHuntersData()
    {
        // Remembering data
        Guid distanceId;
        DateTime loadBeginTime;
        DateTime loadEndTime;
        try
        {
            _huntersDataReloadMutex.WaitOne();

            distanceId = _activeDistance.Id;
            loadBeginTime = _activeDistance.FirstHunterStartTime;
            loadEndTime = _activeDistance.CloseTime;
        }
        finally
        {
            _huntersDataReloadMutex.ReleaseMutex();
        }
        
        // Reloading hunters (some hunters may appear during the run, so we need to reload them)
        IReadOnlyCollection<Hunter> newHunters;

        try
        {
            newHunters = _webClient.MassGetHuntersByDistanceIdWithoutLocationsHistoriesAsync(distanceId).Result;
        }
        catch (Exception)
        {
            Dispatcher.UIThread.InvokeAsync(() => { MarkHuntersDataReloadFailure(); });

            return;
        }

        // As early as possible applying teamless team to hunters without team
        if (_teamsService == null)
        {
            _logger.Error("OLOLO!");
        }
        newHunters = _teamsService.ApplyTeamlessTeamToHunters(newHunters);
        
        // And locations for all of them (we can't reload locations for only filtered ones, because user can change filtering at any time)
        var huntersIdsToReload = newHunters
            .Select(h => h.Id)
            .ToList()
            .AsReadOnly();

        Dictionary<Guid, IReadOnlyCollection<HunterLocation>> newHuntersLocationsData;
        try
        {
            newHuntersLocationsData = _webClient.MassGetHuntersLocationsAsync(
                new HuntersLocationsMassGetRequest(huntersIdsToReload, loadBeginTime, loadEndTime)).Result;
        }
        catch (Exception)
        {
            Dispatcher.UIThread.InvokeAsync(() => { MarkHuntersDataReloadFailure(); });

            return;
        }

        // Generating hunters with locations (to load into distance)
        var newHuntersWithLocations = newHunters
            .Select(h => new Hunter
            (
                h.Id,
                h.Name,
                h.IsRunning,
                h.Team,
                GetHunterLocationFromDictionaryWithLogging(newHuntersLocationsData, h),
                h.Color
            ))
            .ToList()
            .AsReadOnly();

        // Re-filtering hunters
        var newFilteredHunters = ApplyHuntersFilter(newHuntersWithLocations);

        // New hunters to display
        var newHuntersToDisplay = FilterHuntersLocationsByHistoriesInterval(newFilteredHunters); // By interval
        newHuntersToDisplay = GpsFilterHuntersLocations(newHuntersToDisplay); // Filtering GPS noise (!locations IDs and altitudes are lost!)

        // Finally updating external data (if distance still the same)
        try
        {
            _huntersDataReloadMutex.WaitOne();

            if (_activeDistance == null || _activeDistance.Id != distanceId)
            {
                // Distance was changed (or not exist), discarding our data
                Dispatcher.UIThread.InvokeAsync(() =>
                {
                    _logger.Warn($"Distance was changed ({ distanceId } -> { _activeDistance?.Id }), discarding reloaded data.");
                });
                
                Dispatcher.UIThread.InvokeAsync(MarkHuntersDataAsActual);
                SynchronizedInvalidateVisual();

                return;
            }
            
            _activeDistance.UpdateHunters(newHuntersWithLocations);
            _filteredHunters = newFilteredHunters;
            _huntersToDisplay = newHuntersToDisplay;
            
            Dispatcher.UIThread.InvokeAsync(MarkHuntersDataAsActual);
            SynchronizedInvalidateVisual();
        }
        finally
        {
            _huntersDataReloadMutex.ReleaseMutex();
        }
    }

    private IReadOnlyCollection<HunterLocation> GetHunterLocationFromDictionaryWithLogging(
        Dictionary<Guid, IReadOnlyCollection<HunterLocation>> dictionary,
        Hunter hunter)
    {
        if (dictionary.ContainsKey(hunter.Id))
        {
            return dictionary[hunter.Id];
        }
        else
        {
            Dispatcher.UIThread.InvokeAsync(() =>
            {
                _logger.Warn($"Cant't find hunter with name {hunter.Name} ({hunter.Id}) in hunters location dictionary!");
            });

            return hunter.LocationsHistory;
        }
    }


    public void SetHuntersDataReloadInterval(double interval)
    {
        _huntersDataReloadTimer.Interval = interval;
    }

    private void AddDistanceLayer()
    {
        _distanceLayer = new DistanceLayer(_activeDistance, OnDistanceLoadedHandler, SetMapProgressState, _textDrawer, _webClient, DistanceLayerOrder);
        _layers.Add(_distanceLayer);

        OrderLayers();
    }

    private void OrderLayers()
    {
        _layers = _layers
            .OrderBy(l => l.Order)
            .ToList();
    }

    private IReadOnlyCollection<Hunter> GpsFilterHuntersLocations(IReadOnlyCollection<Hunter> hunters)
    {
        if (hunters == null)
        {
            return null;
        }

        return hunters
            .Select(fh => new Hunter
            (
                fh.Id,
                fh.Name,
                fh.IsRunning,
                fh.Team,
                _gpsFilter.FilterLocations(fh.LocationsHistory
                        .Select(l => new GpsLocation(l.Timestamp, l.Lat, l.Lon))
                        .ToList())
                    .Select(fl => new HunterLocation(Guid.Empty, fl.Timestamp.UtcDateTime, fl.Lat, fl.Lon, 0))
                    .ToList(),
                fh.Color
            ))
            .ToList();
    }

    public void SetHuntersLocationsInterval(DateTime beginTime, DateTime endTime)
    {
        if (endTime < beginTime)
        {
            throw new ArgumentException(nameof(endTime), "End time must be no less than start time.");
        }

        _huntersHistoriesBeginTime = beginTime;
        _huntersHistoriesEndTime = endTime;

        var filteredByInterval = FilterHuntersLocationsByHistoriesInterval(_filteredHunters);
        _huntersToDisplay = GpsFilterHuntersLocations(filteredByInterval);

        SynchronizedInvalidateVisual();
    }

    private IReadOnlyCollection<Hunter> FilterHuntersLocationsByHistoriesInterval(IReadOnlyCollection<Hunter> hunters)
    {
        if (hunters == null)
        {
            return null;
        }

        return hunters
            .Select(fh => new Hunter
            (
                fh.Id,
                fh.Name,
                fh.IsRunning,
                fh.Team,
                fh.LocationsHistory
                    .Where(lh =>
                        (lh.Timestamp >= _huntersHistoriesBeginTime) && (lh.Timestamp <= _huntersHistoriesEndTime))
                    .ToList(),
                fh.Color
            ))
            .ToList();
    }

    public void SynchronizedInvalidateVisual()
    {
        try
        {
            _invalidateVisualMutex.WaitOne();
            
            Dispatcher.UIThread.InvokeAsync(InvalidateVisual);
        }
        finally
        {
            _invalidateVisualMutex.ReleaseMutex();
        }
    }

}
