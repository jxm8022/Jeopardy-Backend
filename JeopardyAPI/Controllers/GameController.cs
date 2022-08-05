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
        if (gameui.teams.Count > 0 && gameui.players.Count > 0 && gameui.boardstate.Count > 0) // eventually check for current team with gameui.game.current_team > 0 && 
        {
            // =======================================================
            // getting game id for new saved game
            int game_id = await _bl.CreateGameAsync(gameui.game);
            if (game_id > 0)
            {
                // =======================================================
                // creating the teams to get team ids
                await _bl.CreateTeamsAsync(gameui.teams);
                // getting team ids
                List<Team> allTeams = await _bl.GetTeamsSortedbyScoreAsync(); // can improve way to get team ids
                for (int i = 0; i < gameui.teams.Count; i++)
                {
                    // adding team ids to gameui teams
                    foreach (Team team in allTeams)
                    {
                        if (team.team_name == gameui.teams[i].team_name && team.team_score == gameui.teams[i].team_score)
                        {
                            gameui.teams[i].team_id = team.team_id;
                        }
                    }
                    // adding team ids to players
                    for (int j = 0; j < gameui.players[i].Count; j++)
                    {
                        if (gameui.teams[i].team_id > 0)
                        {
                            gameui.players[i][j].team_id = gameui.teams[i].team_id;
                        }
                    }
                }
                // =======================================================
                // creating the players with team ids in object now
                await _bl.CreatePlayersAsync(gameui.players);
                // =======================================================
                // creating game states which is how we know which teams are playing the game
                List<Gamestate> gamestates = new List<Gamestate>();
                // adding game id and team ids
                foreach (Team team in gameui.teams)
                {
                    Gamestate gamestate = new Gamestate { team_id = team.team_id, game_id = game_id };
                    gamestates.Add(gamestate);
                }
                if (gamestates.Count > 1)
                {
                    await _bl.CreateGamestateAsync(gamestates);
                }
                // =======================================================
                // creating board states which is how we know what questions we had, etc.
                // adding game id
                foreach (Boardstate boardstate in gameui.boardstate)
                {
                    boardstate.game_id = game_id;
                }
                await _bl.CreateBoardstateAsync(gameui.boardstate); // can improve frontend by reading questions and isquestionanswered AND then build boardstate here instead of frontend
                return Ok();
            }
            return NoContent();
        }
        return NoContent();
    }

    [HttpPut("UpdatingSavedGame")]
    public async Task<ActionResult> Put(GameUI gameui)
    {
        if (gameui.game.game_winner > 0) // game is done and we update the team scores and delete the saved games by their game_id
        {
            // =======================================================
            // deleting the boardstates
            await _bl.DeleteBoardstatesAsync(gameui.game.game_id);
            // =======================================================
            // deleting the gamestates
            await _bl.DeleteGamestatesAsync(gameui.game.game_id);
            // =======================================================
            // deleting the game
            await _bl.DeleteGameAsync(gameui.game.game_id);
            // =======================================================
            // updating the team scores
            await _bl.UpdateTeamsAsync(gameui.teams);
            return Ok();
        }
        else // game is not done and we update the teams scores, boardstate, current_player, etc.
        {
            // =======================================================
            // updating the team scores
            await _bl.UpdateTeamsAsync(gameui.teams);
            // =======================================================
            // updating the current player
            await _bl.UpdateGameAsync(gameui.game);
            // =======================================================
            // updating the boardstates
            await _bl.UpdateBoardstatesAsync(gameui.boardstate);
            return Ok();
        }
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