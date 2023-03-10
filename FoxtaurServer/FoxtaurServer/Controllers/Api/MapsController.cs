using FoxtaurServer.Models.Api;
using FoxtaurServer.Models.Api.Requests;
using FoxtaurServer.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace FoxtaurServer.Controllers.Api;

/// <summary>
/// Controller to work with maps
/// </summary>
public class MapsController : Controller
{
    private readonly IMapsService _mapsService;

    public MapsController(IMapsService mapsService)
    {
        _mapsService = mapsService;
    }

    /// <summary>
    /// Mass get maps
    /// </summary>
    [Route("api/Maps/MassGet")]
    [HttpPost]
    public async Task<ActionResult<IReadOnlyCollection<MapDto>>> MassGetMaps([FromBody]MapsMassGetRequest request)
    {
        if (request == null || request.MapsIds == null)
        {
            return BadRequest();
        }

        var result = await _mapsService.MassGetMapsAsync(request.MapsIds);

        return Ok(result);
    }
}