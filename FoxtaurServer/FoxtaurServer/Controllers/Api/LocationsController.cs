using FoxtaurServer.Models.Api;
using FoxtaurServer.Models.Api.Requests;
using FoxtaurServer.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoxtaurServer.Controllers.Api;

/// <summary>
/// Controller to work with locations
/// </summary>
[Authorize]
[ApiController]
public class LocationsController : ControllerBase
{
    private readonly ILocationsService _locationsService;

    public LocationsController(ILocationsService locationsService)
    {
        _locationsService = locationsService;
    }

    /// <summary>
    /// Mass get locations
    /// </summary>
    [AllowAnonymous]
    [Route("api/Locations/MassGet")]
    [HttpPost]
    public async Task<ActionResult<IReadOnlyCollection<LocationDto>>> MassGetLocations([FromBody]LocationsMassGetRequest request)
    {
        if (request == null || request.LocationsIds == null)
        {
            return BadRequest();
        }

        var result = await _locationsService.MassGetLocationsAsync(request.LocationsIds);

        return Ok(result);
    }
    
    /// <summary>
    /// Get all locations
    /// </summary>
    [AllowAnonymous]
    [Route("api/Locations/GetAll")]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<LocationDto>>> GetAllLocations()
    {
        var result = await _locationsService.GetAllLocationsAsync();

        return Ok(result);
    }
    
    /// <summary>
    /// Create new location
    /// </summary>
    [Route("api/Locations/Create")]
    [HttpPost]
    public async Task<ActionResult<LocationDto>> CreateLocation([FromBody]CreateLocationRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Location name must be specified.");
        }

        var newLocation = await _locationsService.CreateNewLocationAsync(new LocationDto(
            Guid.Empty,
            request.Name,
            request.Type,
            request.Lat,
            request.Lon,
            request.FoxId));
        
        if (newLocation == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Generic error during location creation");
        }

        return Ok(newLocation);
    }
}