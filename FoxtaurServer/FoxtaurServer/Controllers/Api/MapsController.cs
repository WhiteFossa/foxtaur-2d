using FoxtaurServer.Models.Api;
using FoxtaurServer.Models.Api.Requests;
using FoxtaurServer.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoxtaurServer.Controllers.Api;

/// <summary>
/// Controller to work with maps
/// </summary>
[Authorize]
[ApiController]
public class MapsController : ControllerBase
{
    private readonly IMapsService _mapsService;

    public MapsController(IMapsService mapsService)
    {
        _mapsService = mapsService;
    }

    /// <summary>
    /// Mass get maps
    /// </summary>
    [AllowAnonymous]
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
    
    /// <summary>
    /// Get all maps
    /// </summary>
    [AllowAnonymous]
    [Route("api/Maps/GetAll")]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<MapDto>>> GetAllMaps()
    {
        var result = await _mapsService.GetAllMapsAsync();

        return Ok(result);
    }
    
    /// <summary>
    /// Create new map
    /// </summary>
    [Route("api/Maps/Create")]
    [HttpPost]
    public async Task<ActionResult<MapDto>> CreateMap([FromBody]CreateMapRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Map name must be specified.");
        }

        var newMap = await _mapsService.CreateNewMapAsync(new MapDto(
            Guid.Empty,
            request.Name,
            request.NorthLat,
            request.SouthLat,
            request.EastLon,
            request.WestLon,
            request.Url,
            request.FileId));
        
        if (newMap == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Generic error during map creation");
        }

        return Ok(newMap);
    }
}