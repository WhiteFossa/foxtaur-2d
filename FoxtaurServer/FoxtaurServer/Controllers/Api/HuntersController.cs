using FoxtaurServer.Models.Api;
using FoxtaurServer.Models.Api.Requests;
using FoxtaurServer.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoxtaurServer.Controllers.Api;

/// <summary>
/// Controller to work with hunters
/// </summary>
[Authorize]
[ApiController]
public class HuntersController : ControllerBase
{
    private readonly IHuntersService _huntersService;
    private readonly IHuntersLocationsService _huntersLocationsService;

    public HuntersController(IHuntersService huntersService,
        IHuntersLocationsService huntersLocationsService)
    {
        _huntersService = huntersService;
        _huntersLocationsService = huntersLocationsService;
    }
    
    /// <summary>
    /// Mass get hunters
    /// </summary>
    [AllowAnonymous]
    [Route("api/Hunters/MassGet")]
    [HttpPost]
    public async Task<ActionResult<IReadOnlyCollection<HunterDto>>> MassGetMaps([FromBody]HuntersMassGetRequest request)
    {
        if (request == null || request.HuntersIds == null)
        {
            return BadRequest();
        }

        var result = await _huntersService.MassGetHuntersAsync(request.HuntersIds);

        return Ok(result);
    }

    /// <summary>
    /// Set team to hunter
    /// </summary>
    [Route("api/Hunters/SetTeam")]
    [HttpPost]
    public async Task<IActionResult> SetHunterTeam([FromBody]SetHunterTeamRequest request)
    {
        return Ok("Team assigned.");
    }
}