using Microsoft.AspNetCore.Mvc;
using Models;
using BusinessLayer;

namespace JeopardyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TeamController : ControllerBase
{
    private readonly IBusiness _bl;
    private readonly ILogger<TeamController> _logger;

    public TeamController(IBusiness bl, ILogger<TeamController> logger)
    {
        _bl = bl;
        _logger = logger;
    }

    [HttpGet("SortedTeams")]
    public async Task<ActionResult<List<Team>>> Get()
    {
        List<Team> teams = await _bl.GetTeamsSortedbyScoreAsync();
        if (teams != null)
        {
            return Ok(teams);
        }
        return NoContent();
    }

    [HttpPut("UpdateTeam")]
    public async Task Put(Team team)
    {
        await _bl.UpdateTeamAsync(team);
    }

    [HttpPost("CreateTeams")]
    public async Task<ActionResult> Post(List<Team> teams)
    {
        if (teams != null && teams.Count > 0)
        {
            await _bl.CreateTeamsAsync(teams);
            return Ok();
        }
        return NoContent();
    }
}
