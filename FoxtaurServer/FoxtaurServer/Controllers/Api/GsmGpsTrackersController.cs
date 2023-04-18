using FoxtaurServer.Models.Api;
using FoxtaurServer.Models.Api.Requests;
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

    /// <summary>
    /// Create new tracker
    /// </summary>
    [Route("api/GsmGpsTrackers/Create")]
    [HttpPost]
    public async Task<ActionResult<GsmGpsTrackerDto>> CreateMap([FromBody] CreateGsmGpsTrackerRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        var newTracker = await _trackersService.CreateNewTrackerAsync(new GsmGpsTrackerDto
        (
            Guid.Empty,
            request.Imei,
            null
        ));
        
        if (newTracker == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Generic error during tracker creation");
        }

        return Ok(newTracker);
    }
}