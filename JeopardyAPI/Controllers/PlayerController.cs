using Microsoft.AspNetCore.Mvc;
using Models;
using BusinessLayer;

namespace JeopardyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PlayerController : ControllerBase
{
    private readonly IBusiness _bl;
    private readonly ILogger<PlayerController> _logger;

    public PlayerController(IBusiness bl, ILogger<PlayerController> logger)
    {
        _bl = bl;
        _logger = logger;
    }

    [HttpGet("GetMembers/{team_id}")]
    public async Task<ActionResult<List<Player>>> Get(int team_id)
    {
        List<Player> players = await _bl.GetTeamMembersAsync(team_id);
        if (players != null)
        {
            return Ok(players);
        }
        return NoContent();
    }

    [HttpPost("CreatePlayers")]
    public async Task<ActionResult> Post(Player player)
    {
        if (player.Name.Length > 0)
        {
            await _bl.CreatePlayerAsync(player);
            return Ok();
        }
        return NoContent();
    }
}
