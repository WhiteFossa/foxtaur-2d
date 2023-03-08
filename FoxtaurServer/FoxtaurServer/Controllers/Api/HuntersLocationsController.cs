using FoxtaurServer.Models.Api;
using FoxtaurServer.Models.Api.Requests;
using FoxtaurServer.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace FoxtaurServer.Controllers.Api;

/// <summary>
/// Controller to work with hunters locations
/// </summary>
public class HuntersLocationsController : Controller
{
    private readonly IHuntersLocationsService _huntersLocationsService;

    public HuntersLocationsController(IHuntersLocationsService huntersLocationsService)
    {
        _huntersLocationsService = huntersLocationsService;
    }
    
    /// <summary>
    /// Get hunter location by Id
    /// </summary>
    [Route("api/HuntersLocations/{id}")]
    [HttpGet]
    public async Task<ActionResult<HunterLocationDto>> GetHunterLocationById(Guid id)
    {
        var hunterLocation = await _huntersLocationsService.GetHunterLocationByIdAsync(id);

        if (hunterLocation == null)
        {
            return NotFound();
        }

        return Ok(hunterLocation);
    }

    /// <summary>
    /// Mass get hunters locations
    /// </summary>
    [Route("api/HuntersLocations/MassGet")]
    [HttpPost]
    public async Task<ActionResult<HuntersLocationsDictionaryDto>> MassGetHuntersLocations([FromBody]HuntersLocationsMassGetRequest request)
    {
        if (request == null || request.HuntersIds == null)
        {
            return BadRequest();
        }
        
        var result = await _huntersLocationsService.MassGetHuntersLocationsAsync(request.HuntersIds, request.FromTime);

        return Ok(result);
    }
}