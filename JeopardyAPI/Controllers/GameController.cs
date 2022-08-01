using Microsoft.AspNetCore.Mvc;
using Models;
using BusinessLayer;

namespace JeopardyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly IBusiness _bl;
    private readonly ILogger<GameController> _logger;

    public GameController(IBusiness bl, ILogger<GameController> logger)
    {
        _bl = bl;
        _logger = logger;
    }

    [HttpGet("GetSavedGames")]
    public async Task<ActionResult<List<GameUI>>> Get()
    {
        List<GameUI> games = await _bl.GetSavedGamesAsync();
        if (games != null)
        {
            return Ok(games);
        }
        return NoContent();
    }
}