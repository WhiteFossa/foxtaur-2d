using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace FoxtaurServer.Controllers.Api;

/// <summary>
/// Controller to work with foxes
/// </summary>
public class FoxesController : Controller
{
    private readonly IFoxesService _foxesService;

    public FoxesController(IFoxesService foxesService)
    {
        _foxesService = foxesService;
    }
    
    /// <summary>
    /// Get fox by Id
    /// </summary>
    [Route("api/Foxes/{id}")]
    [HttpGet]
    public async Task<ActionResult<FoxDto>> GetFoxById(Guid id)
    {
        var fox = await _foxesService.GetFoxByIdAsync(id);

        if (fox == null)
        {
            return NotFound();
        }

        return Ok(fox);
    }
}