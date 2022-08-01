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

    [HttpPost("CreateSavedGame")]
    public async Task<ActionResult> Post(GameUI gameui)
    {
        if (gameui.game.current_team > 0 && gameui.teams.Count > 0 && gameui.questions.Count > 0 && gameui.boardstate.Count > 0)
        {
            int game_id = await _bl.CreateGameAsync(gameui.game);
            if (game_id > 0)
            {
                List<Gamestate> gamestates = new List<Gamestate>();
                foreach (Team team in gameui.teams)
                {
                    Gamestate gamestate = new Gamestate { team_id = team.team_id, game_id = game_id };
                    gamestates.Add(gamestate);
                }
                if (gamestates.Count > 1)
                {
                    await _bl.CreateGamestateAsync(gamestates);
                }
                foreach (Boardstate boardstate in gameui.boardstate)
                {
                    boardstate.game_id = game_id;
                }
                await _bl.CreateBoardstateAsync(gameui.boardstate);
                return Ok();
            }
            return NoContent();
        }
        return NoContent();
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