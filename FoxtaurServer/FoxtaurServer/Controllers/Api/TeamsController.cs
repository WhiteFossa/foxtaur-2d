using FoxtaurServer.Models.Api;
using FoxtaurServer.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace FoxtaurServer.Controllers.Api;

/// <summary>
/// Controller to work with teams
/// </summary>
public class TeamsController : Controller
{
    private readonly ITeamsService _teamsService;

    public TeamsController(ITeamsService teamsService)
    {
        _teamsService = teamsService;
    }
    
    /// <summary>
    /// Get team by Id
    /// </summary>
    [Route("api/Teams/{id}")]
    [HttpGet]
    public async Task<ActionResult<TeamDto>> GetTeamById(Guid id)
    {
        var team = await _teamsService.GetTeamByIdAsync(id);

        if (team == null)
        {
            return NotFound();
        }

        return Ok(team);
    }
}