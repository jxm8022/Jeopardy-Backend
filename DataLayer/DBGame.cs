using System.Data.SqlClient;
using System.Data;
using Models;

namespace DataLayer;

public static class DBGame
{
    public static async Task<int> CreateGame(Game game, string _connectionString)
    {
        return await Task.Factory.StartNew(() =>
        {
            DataSet gameSet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("SELECT current_team FROM Game WHERE game_id = -1", connection);

            SqlDataAdapter gameAdapter = new SqlDataAdapter(cmd);

            gameAdapter.Fill(gameSet, "GameTable");

            DataTable? gameTable = gameSet.Tables["GameTable"];
            if (gameTable != null)
            {
                DataRow newRow = gameTable.NewRow();
                newRow["current_team"] = game.current_team;

                gameTable.Rows.Add(newRow);

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(gameAdapter);
                SqlCommand insert = commandBuilder.GetInsertCommand();

                gameAdapter.InsertCommand = insert;

                gameAdapter.Update(gameTable);
            }

            gameSet = new DataSet();

            using SqlCommand cmd2 = new SqlCommand("SELECT game_id FROM Game ORDER BY game_id DESC", connection);

            gameAdapter = new SqlDataAdapter(cmd2);

            gameAdapter.Fill(gameSet, "GameTable");

            gameTable = gameSet.Tables["GameTable"];
            if (gameTable != null && gameTable.Rows.Count > 0)
            {
                return (int)gameTable.Rows[0]["game_id"];
            }

            return -1;
        });
    }

    public static async Task CreateGamestate(List<Gamestate> gamestates, string _connectionString)
    {
        await Task.Factory.StartNew(() =>
        {
            DataSet gameSet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);

            using SqlCommand cmd = new SqlCommand("SELECT team_id, game_id FROM Gamestate WHERE gamestate_id = -1", connection);

            SqlDataAdapter gameAdapter = new SqlDataAdapter(cmd);

            gameAdapter.Fill(gameSet, "GameTable");

            DataTable? gameTable = gameSet.Tables["GameTable"];
            if (gameTable != null)
            {
                foreach (Gamestate gamestate in gamestates)
                {
                    DataRow newRow = gameTable.NewRow();
                    newRow["team_id"] = gamestate.team_id;
                    newRow["game_id"] = gamestate.game_id;

                    gameTable.Rows.Add(newRow);
                }

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(gameAdapter);
                SqlCommand insert = commandBuilder.GetInsertCommand();

                gameAdapter.InsertCommand = insert;

                gameAdapter.Update(gameTable);
            }
        });
    }

    public static async Task CreateBoardstate(List<Boardstate> boardstates, string _connectionString)
    {
        await Task.Factory.StartNew(() =>
        {
            DataSet boardSet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);

            using SqlCommand cmd = new SqlCommand("SELECT x_position, y_posistion, answered, question_id, game_id FROM Boardstate WHERE boardstate_id = -1", connection);

            SqlDataAdapter boardAdapter = new SqlDataAdapter(cmd);

            boardAdapter.Fill(boardSet, "BoardTable");

            DataTable? boardTable = boardSet.Tables["BoardTable"];
            if (boardTable != null)
            {
                foreach (Boardstate boardstate in boardstates)
                {
                    DataRow newRow = boardTable.NewRow();
                    newRow["x_position"] = boardstate.x_position;
                    newRow["y_posistion"] = boardstate.y_position;
                    newRow["answered"] = boardstate.answered;
                    newRow["question_id"] = boardstate.question_id;
                    newRow["game_id"] = boardstate.game_id;

                    boardTable.Rows.Add(newRow);
                }

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(boardAdapter);
                SqlCommand insert = commandBuilder.GetInsertCommand();

                boardAdapter.InsertCommand = insert;

                boardAdapter.Update(boardTable);
            }
        });
    }

    public static async Task<List<GameUI>> GetSavedGames(string _connectionString)
    {
        return await Task.Factory.StartNew(() =>
        {
            List<GameUI> gamesui = new List<GameUI>();

            // get gamesui stuff
            foreach (Game game in GetGames(_connectionString))
            {
                GameUI gameui = new GameUI();
                gameui.game = game;
                gameui.teams = GetTeams(game.game_id, _connectionString);
                gameui.subcategories = GetSubCats(game.game_id, _connectionString);
                gameui.questions = GetQuestions(game.game_id, _connectionString);
                gameui.boardstate = GetBoardState(game.game_id, _connectionString);
                gamesui.Add(gameui);
            }

            return gamesui;
        });
    }

    private static List<Game> GetGames(string _connectionString)
    {
        List<Game> games = new List<Game>();
        DataSet gameSet = new DataSet();

        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand("SELECT * FROM Game WHERE game_winner < 1", connection);

        SqlDataAdapter gameAdapter = new SqlDataAdapter(cmd);

        gameAdapter.Fill(gameSet, "GameTable");

        DataTable? gameTable = gameSet.Tables["GameTable"];
        if (gameTable != null && gameTable.Rows.Count > 0)
        {
            foreach (DataRow row in gameTable.Rows)
            {
                Game game = new Game
                {
                    game_id = (int)row["game_id"],
                    game_winner = (int)row["game_winner"],
                    current_team = (int)row["current_team"]
                };
                games.Add(game);
            }
            return games;
        }
        return null!;
    }

    private static List<Team> GetTeams(int game_id, string _connectionString)
    {
        List<Team> teams = new List<Team>();
        DataSet teamSet = new DataSet();

        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand("SELECT game_id, Team.team_id, Team.team_name, Team.team_score FROM Gamestate INNER JOIN Team ON Gamestate.team_id = Team.team_id WHERE game_id = @game_id", connection);
        cmd.Parameters.AddWithValue("@game_id", game_id);

        SqlDataAdapter teamAdapter = new SqlDataAdapter(cmd);

        teamAdapter.Fill(teamSet, "TeamTable");

        DataTable? teamTable = teamSet.Tables["TeamTable"];
        if (teamTable != null && teamTable.Rows.Count > 0)
        {
            foreach (DataRow row in teamTable.Rows)
            {
                Team team = new Team
                {
                    team_id = (int)row["team_id"],
                    team_name = (string)row["team_name"],
                    team_score = (int)row["team_score"]
                };
                teams.Add(team);
            }
            return teams;
        }
        return null!;
    }

    private static List<Subcategory> GetSubCats(int game_id, string _connectionString)
    {
        List<Subcategory> subcategories = new List<Subcategory>();
        DataSet subcatSet = new DataSet();

        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand("SELECT subcategory_id, subcategory_name, Subcategory.category_id FROM Subcategory INNER JOIN (SELECT Question.category_id FROM Boardstate INNER JOIN Question ON Boardstate.question_id = Question.question_id WHERE game_id = @game_id GROUP BY Question.category_id) AS Categories ON Categories.category_id = Subcategory.subcategory_id", connection);
        cmd.Parameters.AddWithValue("@game_id", game_id);

        SqlDataAdapter subcatAdapter = new SqlDataAdapter(cmd);

        subcatAdapter.Fill(subcatSet, "SubcatTable");

        DataTable? subcatTable = subcatSet.Tables["SubcatTable"];
        if (subcatTable != null && subcatTable.Rows.Count > 0)
        {
            foreach (DataRow row in subcatTable.Rows)
            {
                Subcategory subcategory = new Subcategory
                {
                    subcategory_id = (int)row["subcategory_id"],
                    subcategory_name = (string)row["subcategory_name"],
                    category_id = (int)row["category_id"]
                };
                subcategories.Add(subcategory);
            }
            return subcategories;
        }
        return null!;
    }

    private static List<QA> GetQuestions(int game_id, string _connectionString)
    {
        List<QA> questions = new List<QA>();
        DataSet questionSet = new DataSet();

        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand("SELECT Questions.question_id, question_entry, category_id, answer_id, answer_entry, Answer.question_id FROM (SELECT Question.question_id, question_entry, category_id FROM Boardstate INNER JOIN Question ON Boardstate.question_id = Question.question_id WHERE game_id = @game_id) AS Questions INNER JOIN Answer ON Questions.question_id = Answer.question_id", connection);
        cmd.Parameters.AddWithValue("@game_id", game_id);

        SqlDataAdapter questionAdapter = new SqlDataAdapter(cmd);

        questionAdapter.Fill(questionSet, "QuestionTable");

        DataTable? questionTable = questionSet.Tables["QuestionTable"];
        if (questionTable != null && questionTable.Rows.Count > 0)
        {
            foreach (DataRow row in questionTable.Rows)
            {
                QA qa = new QA
                {
                    question = new Question
                    {
                        question_id = (int)row["question_id"],
                        question_entry = (string)row["question_entry"],
                        category_id = (int)row["category_id"]
                    },
                    answer = new Answer
                    {
                        answer_id = (int)row["answer_id"],
                        answer_entry = (string)row["answer_entry"],
                        question_id = (int)row["question_id"]
                    }
                };
                questions.Add(qa);
            }
            return questions;
        }
        return null!;
    }

    private static List<Boardstate> GetBoardState(int game_id, string _connectionString)
    {
        List<Boardstate> boardstates = new List<Boardstate>();
        DataSet boardSet = new DataSet();

        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand("SELECT * FROM Boardstate WHERE game_id = @game_id", connection);
        cmd.Parameters.AddWithValue("@game_id", game_id);

        SqlDataAdapter boardAdapter = new SqlDataAdapter(cmd);

        boardAdapter.Fill(boardSet, "BoardTable");

        DataTable? boardTable = boardSet.Tables["BoardTable"];
        if (boardTable != null && boardTable.Rows.Count > 0)
        {
            foreach (DataRow row in boardTable.Rows)
            {
                Boardstate boardstate = new Boardstate
                {
                    boardstate_id = (int)row["boardstate_id"],
                    x_position = (int)row["x_position"],
                    y_position = (int)row["y_posistion"],
                    answered = (bool)row["answered"],
                    question_id = (int)row["question_id"],
                    game_id = (int)row["game_id"]
                };
                boardstates.Add(boardstate);
            }
            return boardstates;
        }
        return null!;
    }

    public static async Task DeleteBoardstates(int game_id, string _connectionString)
    {
        await Task.Factory.StartNew(() =>
        {
            DataSet boardSet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);

            using SqlCommand cmd = new SqlCommand("DELETE FROM Boardstate WHERE game_id = @game_id", connection);
            cmd.Parameters.AddWithValue("@game_id", game_id);

            SqlDataAdapter boardAdapter = new SqlDataAdapter(cmd);

            boardAdapter.Fill(boardSet, "BoardTable");

            DataTable? boardTable = boardSet.Tables["BoardTable"];
            if (boardTable != null)
            {

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(boardAdapter);
                SqlCommand delete = commandBuilder.GetDeleteCommand();

                boardAdapter.DeleteCommand = delete;

                boardAdapter.Update(boardTable);
            }
        });
    }
    public static async Task DeleteGamestates(int game_id, string _connectionString)
    {
        await Task.Factory.StartNew(() =>
        {
            DataSet gameSet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);

            using SqlCommand cmd = new SqlCommand("DELETE FROM Gamestate WHERE game_id = @game_id", connection);
            cmd.Parameters.AddWithValue("@game_id", game_id);

            SqlDataAdapter gameAdapter = new SqlDataAdapter(cmd);

            gameAdapter.Fill(gameSet, "GameTable");

            DataTable? gameTable = gameSet.Tables["GameTable"];
            if (gameTable != null)
            {

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(gameAdapter);
                SqlCommand delete = commandBuilder.GetDeleteCommand();

                gameAdapter.DeleteCommand = delete;

                gameAdapter.Update(gameTable);
            }
        });
    }
    public static async Task DeleteGame(int game_id, string _connectionString)
    {
        await Task.Factory.StartNew(() =>
        {
            DataSet gameSet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);

            using SqlCommand cmd = new SqlCommand("DELETE FROM Game WHERE game_id = @game_id", connection);
            cmd.Parameters.AddWithValue("@game_id", game_id);

            SqlDataAdapter gameAdapter = new SqlDataAdapter(cmd);

            gameAdapter.Fill(gameSet, "GameTable");

            DataTable? gameTable = gameSet.Tables["GameTable"];
            if (gameTable != null)
            {

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(gameAdapter);
                SqlCommand delete = commandBuilder.GetDeleteCommand();

                gameAdapter.DeleteCommand = delete;

                gameAdapter.Update(gameTable);
            }
        });
    }
    public static async Task UpdateGame(Game game, string _connectionString)
    {
        await Task.Factory.StartNew(() =>
        {
            DataSet gameSet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("SELECT * FROM Game WHERE game_id = @game_id", connection);
            cmd.Parameters.AddWithValue("@game_id", game.game_id);

            SqlDataAdapter gameAdapter = new SqlDataAdapter(cmd);

            gameAdapter.Fill(gameSet, "GameTable");

            DataTable? gameTable = gameSet.Tables["GameTable"];
            if (gameTable != null && gameTable.Rows.Count > 0)
            {
                DataColumn[] dt = new DataColumn[1];
                dt[0] = gameTable.Columns["game_id"]!;
                gameTable.PrimaryKey = dt;
                DataRow? gameRow = gameTable.Rows.Find(game.game_id);
                if (gameRow != null)
                {
                    gameRow["game_winner"] = game.game_winner;
                    gameRow["current_team"] = game.current_team;
                }

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(gameAdapter);
                SqlCommand updateCmd = commandBuilder.GetUpdateCommand();

                gameAdapter.UpdateCommand = updateCmd;
                gameAdapter.Update(gameTable);
            }
        });
    }
    public static async Task UpdateBoardstates(List<Boardstate> boardstates, string _connectionString)
    {
        await Task.Factory.StartNew(() =>
        {
            foreach (Boardstate boardstate in boardstates)
            {
                UpdateBoardstate(boardstate, _connectionString);
            }
        });
    }
    private static void UpdateBoardstate(Boardstate boardstate, string _connectionString)
    {
        DataSet boardSet = new DataSet();

        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand("SELECT * FROM Boardstate WHERE boardstate_id = @boardstate_id", connection);
        cmd.Parameters.AddWithValue("@boardstate_id", boardstate.boardstate_id);

        SqlDataAdapter boardAdapter = new SqlDataAdapter(cmd);

        boardAdapter.Fill(boardSet, "BoardTable");

        DataTable? boardTable = boardSet.Tables["BoardTable"];
        if (boardTable != null && boardTable.Rows.Count > 0)
        {
            DataColumn[] dt = new DataColumn[1];
            dt[0] = boardTable.Columns["boardstate_id"]!;
            boardTable.PrimaryKey = dt;
            DataRow? boardRow = boardTable.Rows.Find(boardstate.boardstate_id);
            if (boardRow != null)
            {
                if (boardstate.answered)
                    boardRow["answered"] = 1;
                else
                    boardRow["answered"] = 0;
            }

            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(boardAdapter);
            SqlCommand updateCmd = commandBuilder.GetUpdateCommand();

            boardAdapter.UpdateCommand = updateCmd;
            boardAdapter.Update(boardTable);
        }
    }
}