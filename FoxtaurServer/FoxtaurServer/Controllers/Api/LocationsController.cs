using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace FoxtaurServer.Controllers.Api;

/// <summary>
/// Controller to work with locations
/// </summary>
public class LocationsController : Controller
{
    private readonly ILocationsService _locationsService;

    public LocationsController(ILocationsService locationsService)
    {
        _locationsService = locationsService;
    }
    
    /// <summary>
    /// Get location by Id
    /// </summary>
    [Route("api/Locations/{id}")]
    [HttpGet]
    public async Task<ActionResult<LocationDto>> GetLocationById(Guid id)
    {
        var location = await _locationsService.GetLocationByIdAsync(id);

        if (location == null)
        {
            return NotFound();
        }

        return Ok(location);
    }
}