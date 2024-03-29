using System.Security.Claims;
using FoxtaurServer.Dao.Models;
using FoxtaurServer.Models.Api;
using FoxtaurServer.Models.Api.Enums;
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
    public async Task<ActionResult<IReadOnlyCollection<HunterDto>>> MassGetHunters([FromBody]HuntersMassGetRequest request)
    {
        if (request == null || request.HuntersIds == null)
        {
            return BadRequest();
        }

        var result = await _huntersService.MassGetHuntersAsync(request.HuntersIds);

        return Ok(result);
    }
    
    /// <summary>
    /// Mass get profiles
    /// </summary>
    [AllowAnonymous]
    [Route("api/Hunters/Profiles/MassGet")]
    [HttpPost]
    public async Task<ActionResult<IReadOnlyCollection<ProfileDto>>> MassGetProfiles([FromBody]ProfilesMassGetRequest request)
    {
        if (request == null || request.HuntersIds == null)
        {
            return BadRequest();
        }

        var result = await _huntersService.MassGetHuntersProfilesAsync(request.HuntersIds);

        return Ok(result);
    }

    /// <summary>
    /// Update profile
    /// </summary>
    [Route("api/Hunters/Profiles/Update")]
    [HttpPost]
    public async Task<ActionResult<ProfileDto>> UpdateProfile([FromBody]ProfileUpdateRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }
        
        var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

        var result = await _huntersService.UpdateHunterProfileAsync(request, Guid.Parse(currentUser.Id));

        return Ok(result);
    }

    /// <summary>
    /// Register on distance
    /// </summary>
    [Route("api/Hunters/RegisterOnDistance")]
    [HttpPost]
    public async Task<ActionResult<RegistrationOnDistanceResponseDto>> RegisterOnDistance([FromBody]RegisterOnDistanceRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
        
        var result = await _huntersService.RegisterOnDistanceAsync(request, Guid.Parse(currentUser.Id));
        
        return Ok(result);
    }
}