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
}
