using FoxtaurServer.Models.Api;
using FoxtaurServer.Models.Api.Requests;
using FoxtaurServer.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoxtaurServer.Controllers.Api;

/// <summary>
/// Controller to work with mapfiles
/// </summary>
[Authorize]
[ApiController]
public class MapFilesController : ControllerBase
{
    private readonly IMapFilesService _mapFilesService;

    public MapFilesController(IMapFilesService mapFilesService)
    {
        _mapFilesService = mapFilesService;
    }
    
    /// <summary>
    /// Create new map file
    /// </summary>
    [Route("api/MapFiles/Create")]
    [HttpPost]
    public async Task<ActionResult<MapFileDto>> CreateMapFile([FromBody]CreateMapFileRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Map file name must be specified.");
        }

        var newMapFile = await _mapFilesService.CreateNewMapFileAsync(new MapFileDto(
            Guid.Empty,
            request.Name),
            request.Size);

        if (newMapFile == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Generic error during map file creation");
        }

        return Ok(newMapFile);
    }
}