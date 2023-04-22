using FoxtaurServer.Models.Api;
using FoxtaurServer.Models.Api.Requests;
using FoxtaurServer.Models.Identity;
using FoxtaurServer.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    private readonly UserManager<User> _userManager;

    public GsmGpsTrackersController(IGsmGpsTrackersService trackersService,
        UserManager<User> userManager)
    {
        _trackersService = trackersService;
        _userManager = userManager;
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
        
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Tracker name must be specified.");
        }

        var newTracker = await _trackersService.CreateNewTrackerAsync(new GsmGpsTrackerDto
        (
            Guid.Empty,
            request.Imei,
            request.Name,
            null
        ));
        
        if (newTracker == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Generic error during tracker creation");
        }

        return Ok(newTracker);
    }

    /// <summary>
    /// Claim tracker
    /// </summary>
    /// <returns></returns>
    [Route("api/GsmGpsTrackers/Claim")]
    [HttpPost]
    public async Task<ActionResult<GsmGpsTrackerDto>> ClaimTracker([FromBody] ClaimTrackerRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }
        
        var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
        var associatedTracker = await _trackersService.ClaimTrackerAsync(Guid.Parse(currentUser.Id), request.TrackerId);

        if (associatedTracker == null)
        {
            return NotFound("Tracker with given ID is not found.");
        }

        return Ok(associatedTracker);
    }

    /// <summary>
    /// Delete tracker
    /// </summary>
    [Route("api/GsmGpsTrackers/Delete")]
    [HttpDelete]
    public async Task<ActionResult> DeleteTracker([FromBody] DeleteTrackerRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        try
        {
            await _trackersService.DeleteTrackerAsync(request.TrackerId);
        }
        catch (ArgumentException)
        {
            return BadRequest("Tracker with given ID is not found.");
        }

        return Ok();
    }
}