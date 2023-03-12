using FoxtaurServer.Models.Api;
using FoxtaurServer.Models.Api.Requests;
using FoxtaurServer.Models.Identity;
using FoxtaurServer.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoxtaurServer.Controllers.Api;

/// <summary>
/// Controller to work with hunters locations
/// </summary>
[Authorize]
[ApiController]
public class HuntersLocationsController : ControllerBase
{
    private readonly IHuntersLocationsService _huntersLocationsService;
    private readonly UserManager<User> _userManager;

    public HuntersLocationsController(IHuntersLocationsService huntersLocationsService,
        UserManager<User> userManager)
    {
        _huntersLocationsService = huntersLocationsService;
        _userManager = userManager;
    }
    
    /// <summary>
    /// Mass get hunters locations
    /// </summary>
    [AllowAnonymous]
    [Route("api/HuntersLocations/MassGet")]
    [HttpPost]
    public async Task<ActionResult<HuntersLocationsDictionaryDto>> MassGetHuntersLocations([FromBody]HuntersLocationsMassGetRequest request)
    {
        if (request == null || request.HuntersIds == null)
        {
            return BadRequest();
        }
        
        var result = await _huntersLocationsService.MassGetHuntersLocationsAsync(request.HuntersIds, request.FromTime);

        return Ok(new HuntersLocationsDictionaryDto(result));
    }
    
    /// <summary>
    /// Create new hunter locations
    /// </summary>
    [Route("api/HuntersLocations/MassCreate")]
    [HttpPost]
    public async Task<ActionResult<IReadOnlyCollection<HunterLocationDto>>> CreateHunterLocations([FromBody]CreateHunterLocationRequest request)
    {
        if (request == null || request.HunterLocations == null)
        {
            return BadRequest();
        }

        var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);

        var newLocations = await _huntersLocationsService.MassCreateHuntersLocationsAsync(request.HunterLocations, Guid.Parse(currentUser.Id));
        
        return Ok(newLocations);
    }
}