using FoxtaurServer.Models.Api;
using FoxtaurServer.Models.Api.Requests;
using FoxtaurServer.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoxtaurServer.Controllers.Api;

/// <summary>
/// Controller to work with distances
/// </summary>
[Authorize]
[ApiController]
public class DistancesController : ControllerBase
{
    private readonly IDistancesService _distancesService;

    public DistancesController(IDistancesService distancesService)
    {
        _distancesService = distancesService;
    }
    
    /// <summary>
    /// List distances
    /// </summary>
    [AllowAnonymous]
    [Route("api/Distances/Index")]
    [HttpGet]
    public async Task<IReadOnlyCollection<DistanceDto>> Index()
    {
        return await _distancesService.ListDistancesAsync();
    }

    /// <summary>
    /// Get distance by Id
    /// </summary>
    [AllowAnonymous]
    [Route("api/Distances/{id}")]
    [HttpGet]
    public async Task<ActionResult<DistanceDto>> GetDistanceById(Guid id)
    {
        var distance = await _distancesService.GetDistanceById(id);

        if (distance == null)
        {
            return NotFound();
        }

        return Ok(distance);
    }
    
    /// <summary>
    /// Create new distance
    /// </summary>
    [Route("api/Distances/Create")]
    [HttpPost]
    public async Task<ActionResult<DistanceDto>> CreateDistance([FromBody]CreateDistanceRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Distance name must be specified.");
        }

        var newDistance = await _distancesService.CreateNewDistanceAsync(new DistanceDto(
            Guid.Empty,
            request.Name,
            request.MapId,
            false, // On creation distance is not active
            request.StartLocationId,
            request.FinishCorridorEntranceLocationId,
            request.FinishLocationId,
            request.FoxesLocationsIds,
            new List<Guid>(), // Hunters will be added after distance creation
            request.FirstHunterStartTime,
            request.CloseTime));
        
        if (newDistance == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Generic error during team creation");
        }

        return Ok(newDistance);
    }
}