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
    private readonly IHuntersLocationsService _huntersLocationsService;

    public HuntersController(IHuntersService huntersService,
        IHuntersLocationsService huntersLocationsService)
    {
        _huntersService = huntersService;
        _huntersLocationsService = huntersLocationsService;
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

    /// <summary>
    /// Get hunter locations history by hunter ID
    /// </summary>
    [Route("api/Hunters/{id}/LocationsHistory")]
    [HttpGet]
    public async Task<ActionResult<HunterDto>> GetHunterLocationsHistoryById(Guid id)
    {
        var locationsHistory = await _huntersLocationsService.GetHunterLocationsHistoryByHunterId(id);

        if (locationsHistory == null)
        {
            return NotFound();
        }

        return Ok(locationsHistory);
    }
}