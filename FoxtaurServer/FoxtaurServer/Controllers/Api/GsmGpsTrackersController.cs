using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoxtaurServer.Controllers.Api;

/// <summary>
/// Controller to work with GSM-interfaced trackers
/// </summary>
[Authorize]
[ApiController]
public class GsmGpsTrackersController : ControllerBase
{
    private readonly IGsmGpsTrackersService _trackersService;

    public GsmGpsTrackersController(IGsmGpsTrackersService trackersService)
    {
        _trackersService = trackersService;
    }
    
    /// <summary>
    /// List trackers
    /// </summary>
    [Route("api/GsmGpsTrackers/Index")]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<GsmGpsTrackerDto>>> Index()
    {
        var result = await _trackersService.GetAllTrackersAsync();

        return Ok(result);
    }
}