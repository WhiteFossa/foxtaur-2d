using FoxtaurServer.Models.Api;
using FoxtaurServer.Models.Api.Requests;
using FoxtaurServer.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace FoxtaurServer.Controllers.Api;

/// <summary>
/// Controller to work with locations
/// </summary>
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
}