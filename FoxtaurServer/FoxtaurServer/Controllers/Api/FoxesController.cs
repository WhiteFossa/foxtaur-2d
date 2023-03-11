using FoxtaurServer.Models.Api;
using FoxtaurServer.Models.Api.Requests;
using FoxtaurServer.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoxtaurServer.Controllers.Api;

/// <summary>
/// Controller to work with foxes
/// </summary>
[Authorize]
[ApiController]
public class FoxesController : ControllerBase
{
    private readonly IFoxesService _foxesService;

    public FoxesController(IFoxesService foxesService)
    {
        _foxesService = foxesService;
    }
    
    /// <summary>
    /// Mass get foxes
    /// </summary>
    [AllowAnonymous]
    [Route("api/Foxes/MassGet")]
    [HttpPost]
    public async Task<ActionResult<IReadOnlyCollection<FoxDto>>> MassGetFoxes([FromBody]FoxesMassGetRequest request)
    {
        if (request == null || request.FoxesIds == null)
        {
            return BadRequest();
        }

        var result = await _foxesService.MassGetFoxesAsync(request.FoxesIds);

        return Ok(result);
    }
    
    /// <summary>
    /// Get all foxes
    /// </summary>
    [AllowAnonymous]
    [Route("api/Foxes/GetAll")]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<FoxDto>>> GetAllFoxes()
    {
        var result = await _foxesService.GetAllFoxesAsync();

        return Ok(result);
    }
    
    /// <summary>
    /// Create new fox
    /// </summary>
    [Route("api/Foxes/Create")]
    [HttpPost]
    public async Task<ActionResult<FoxDto>> CreateFox([FromBody]CreateFoxRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Fox name must be specified.");
        }

        var newFox = await _foxesService.CreateNewFoxAsync(new FoxDto(
            Guid.Empty,
            request.Name,
            request.Frequency,
            request.Code));
        
        if (newFox == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Generic error during fox creation");
        }

        return Ok(newFox);
    }
}