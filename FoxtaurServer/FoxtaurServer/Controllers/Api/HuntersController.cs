using System.Security.Claims;
using FoxtaurServer.Models.Api;
using FoxtaurServer.Models.Api.Requests;
using FoxtaurServer.Models.Identity;
using FoxtaurServer.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
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
    private readonly UserManager<User> _userManager;

    public HuntersController(IHuntersService huntersService,
        UserManager<User> userManager)
    {
        _huntersService = huntersService;
        _userManager = userManager;
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

    /*/// <summary>
    /// Set team to hunter
    /// </summary>
    [Route("api/Hunters/SetTeam")]
    [HttpPost]
    public async Task<IActionResult> SetHunterTeam([FromBody]SetHunterTeamRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        var userName = User.Identity.Name;
        var user = await _userManager.FindByNameAsync(userName);

        var result = await _huntersService.AssignTeamToHunter(user.Id, request.TeamId);
        if (result)
        {
            return Ok("Team assigned.");
        }

        return BadRequest("Team not assigned.");
    }*/
}