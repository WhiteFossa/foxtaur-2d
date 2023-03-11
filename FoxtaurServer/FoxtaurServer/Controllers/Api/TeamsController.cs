using FoxtaurServer.Models.Api;
using FoxtaurServer.Models.Api.Requests;
using FoxtaurServer.Services.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FoxtaurServer.Controllers.Api;

/// <summary>
/// Controller to work with teams
/// </summary>
[Authorize]
[ApiController]
public class TeamsController : ControllerBase
{
    private readonly ITeamsService _teamsService;

    public TeamsController(ITeamsService teamsService)
    {
        _teamsService = teamsService;
    }
    
    /// <summary>
    /// Mass get teams
    /// </summary>
    [AllowAnonymous]
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
    
    /// <summary>
    /// Get all existing teams
    /// </summary>
    [AllowAnonymous]
    [Route("api/Teams/GetAll")]
    [HttpGet]
    public async Task<ActionResult<IReadOnlyCollection<TeamDto>>> GetAllTeams()
    {
        var result = await _teamsService.GetAllTeamsAsync();

        return Ok(result);
    }
    
    /// <summary>
    /// Create new team
    /// </summary>
    [Route("api/Teams/Create")]
    [HttpPost]
    public async Task<ActionResult<TeamDto>> CreateTeam([FromBody]CreateTeamRequest request)
    {
        if (request == null)
        {
            return BadRequest();
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Team name must be specified.");
        }

        var newTeam = await _teamsService.CreateNewTeamAsync(new TeamDto(Guid.Empty, request.Name, request.Color));
        if (newTeam == null)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, $"Generic error during team creation");
        }

        return Ok(newTeam);
    }
}