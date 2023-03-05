using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace FoxtaurServer.Controllers.Api;

/// <summary>
/// Controller to work with distances
/// </summary>
public class DistancesController : Controller
{
    private readonly IDistancesService _distancesService;

    public DistancesController(IDistancesService distancesService)
    {
        _distancesService = distancesService;
    }
    
    /// <summary>
    /// List distances
    /// </summary>
    [Route("api/Distances/Index")]
    [HttpGet]
    public async Task<IReadOnlyCollection<DistanceDto>> Index()
    {
        return await _distancesService.ListDistancesAsync();
    }

    /// <summary>
    /// Get distance by Id
    /// </summary>
    [Route("api/Distances/{id}")]
    [HttpGet]
    public async Task<ActionResult<DistanceDto>> GetDistanceById(Guid id)
    {
        var distance = await _distancesService.GetDistanceById(id);

        if (distance == null)
        {
            return NotFound();
        }

        return Ok(distance);
    }
}