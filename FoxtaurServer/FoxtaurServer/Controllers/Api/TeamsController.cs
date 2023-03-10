using FoxtaurServer.Models.Api;
using FoxtaurServer.Models.Api.Requests;
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
    /// Mass get teams
    /// </summary>
    [Route("api/Teams/MassGet")]
    [HttpPost]
    public async Task<ActionResult<IReadOnlyCollection<TeamDto>>> MassGetTeams([FromBody]TeamsMassGetRequest request)
    {
        if (request == null || request.TeamsIds == null)
        {
            return BadRequest();
        }

        var result = await _teamsService.MassGetTeamsAsync(request.TeamsIds);

        return Ok(result);
    }
}