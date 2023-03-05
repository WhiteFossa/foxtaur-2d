using FoxtaurServer.Models.Api;
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
    /// Get map by Id
    /// </summary>
    [Route("api/Maps/{id}")]
    [HttpGet]
    public async Task<ActionResult<MapDto>> GetMapById(Guid id)
    {
        var map = await _mapsService.GetMapByIdAsync(id);

        if (map == null)
        {
            return NotFound();
        }

        return Ok(map);
    }
}