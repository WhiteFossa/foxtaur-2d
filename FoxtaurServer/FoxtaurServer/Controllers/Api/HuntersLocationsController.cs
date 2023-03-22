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
        
        var result = await _huntersLocationsService.MassGetHuntersLocationsAsync(request.HuntersIds, request.FromTime, request.ToTime);

        return Ok(new HuntersLocationsDictionaryDto(result));
    }
    
    /// <summary>
    /// Create new hunter locations. Returns created locations IDs
    /// </summary>
    [Route("api/HuntersLocations/MassCreate")]
    [HttpPost]
    public async Task<ActionResult<IReadOnlyCollection<Guid>>> CreateHunterLocations([FromBody]CreateHunterLocationsRequest request)
    {
        if (request == null || request.HunterLocations == null)
        {
            return BadRequest();
        }

        var currentUser = await _userManager.FindByNameAsync(User.Identity.Name);
        var currentUserId = Guid.Parse(currentUser.Id);
        
        var result = await _huntersLocationsService.MassCreateHuntersLocationsAsync(request.HunterLocations, currentUserId);
        
        return Ok(result);
    }
}