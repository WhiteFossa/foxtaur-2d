using FoxtaurServer.Models.Api;
using FoxtaurServer.Models.Api.Requests;
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
    /// Mass get foxes
    /// </summary>
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
}