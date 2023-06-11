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
    public async Task<ActionResult<MapFileDto>> CreateMapFile([FromBody] CreateMapFileRequest request)
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
                request.Name,
                false),
            request.Size);

        if (newMapFile == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Generic error during map file creation");
        }

        return Ok(newMapFile);
    }

    /// <summary>
    /// Upload part of map file
    /// </summary>
    [Route("api/MapFiles/UploadPart")]
    [HttpPost]
    public async Task<ActionResult> UploadPart([FromBody] UploadMapFilePartRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        if (request.StartPosition < 0)
        {
            return BadRequest("Start position mustn't be negative.");
        }

        if (string.IsNullOrWhiteSpace(request.Data))
        {
            return BadRequest("Data must not be empty.");
        }

        var data = Convert.FromBase64String(request.Data);

        await _mapFilesService.UploadMapFilePartAsync(request.Id, request.StartPosition, data);

        return Ok();
    }

    /// <summary>
    /// Get all map files
    /// </summary>
    [Route("api/MapFiles/GetAll")]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<MapFileDto>>> GetAllMapFilesAsync()
    {
        var result = await _mapFilesService.GetAllMapFilesAsync();

        return Ok(result);
    }

    /// <summary>
    /// Mark map file as ready
    /// </summary>
    [Route("api/MapFiles/MarkAsReady")]
    [HttpPost]
    public async Task<ActionResult> MarkAsReady([FromBody] MarkMapFileAsReadyRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        try
        {
            await _mapFilesService.MarkMapFileAsReadyAsync(request.Id);
        }
        catch (Exception)
        {
            return BadRequest();
        }

        return Ok();
    }
}