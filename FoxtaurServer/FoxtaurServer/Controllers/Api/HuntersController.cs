using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace FoxtaurServer.Controllers.Api;

/// <summary>
/// Controller to work with hunters
/// </summary>
public class HuntersController : Controller
{
    private readonly IHuntersService _huntersService;

    public HuntersController(IHuntersService huntersService)
    {
        _huntersService = huntersService;
    }
    
    /// <summary>
    /// Get hunter by Id
    /// </summary>
    [Route("api/Hunters/{id}")]
    [HttpGet]
    public async Task<ActionResult<HunterDto>> GetHunterById(Guid id)
    {
        var hunter = await _huntersService.GetHunterByIdAsync(id);

        if (hunter == null)
        {
            return NotFound();
        }

        return Ok(hunter);
    }
}