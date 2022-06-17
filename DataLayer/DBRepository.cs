using System.Data.SQLite;
using Models;

namespace DataLayer;

public class DBRepository : IRepository
{
    private readonly string _connectionString;

    public DBRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<QA>> GetQuestionsAsync(int category)
    {
        List<QA> questions = new List<QA>();

        SQLiteConnection dbConnection = new SQLiteConnection(_connectionString);
        dbConnection.Open();

        string sql = "SELECT * FROM question INNER JOIN answer ON question.question_id = answer.question_id WHERE type_id = @category ORDER BY RANDOM() LIMIT 5";
        SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
        command.Parameters.AddWithValue("@category", category);
        SQLiteDataReader reader = command.ExecuteReader();
        while (await reader.ReadAsync())
        {
            questions.Add(new QA
            {
                Question = new Question
                {
                    Id = reader.GetInt32(0),
                    Entry = reader.GetString(1),
                    Type_id = reader.GetInt32(2)
                },
                Answer = new Answer
                {
                    Id = reader.GetInt32(3),
                    Entry = reader.GetString(4),
                    Question_id = reader.GetInt32(5)
                }
            });
        }
        reader.Close();

        dbConnection.Close();

        return questions;
    }
    public async Task CreateTeamsAsync(List<Team> teams)
    {
        SQLiteConnection dbConnection = new SQLiteConnection(_connectionString);
        dbConnection.Open();

        string sql = "INSERT INTO team(team_name, score) VALUES";
        for (int i = 0; i < teams.Count; i++)
        {
            if (i == teams.Count - 1)
                sql += $"(@team_name_{i}, @score_{i})";
            else
                sql += $"(@team_name_{i}, @score_{i}),";
        }
        SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
        for (int i = 0; i < teams.Count; i++)
        {
            command.Parameters.AddWithValue($"team_name_{i}", teams[i].Name);
            command.Parameters.AddWithValue($"score_{i}", teams[i].Score);
        }
        await command.ExecuteNonQueryAsync();

        dbConnection.Close();
    }
    public async Task UpdateTeamAsync(Team team)
    {
        SQLiteConnection dbConnection = new SQLiteConnection(_connectionString);
        dbConnection.Open();

        string sql = "UPDATE team SET score = @score WHERE team_id = @team_id";
        SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
        command.Parameters.AddWithValue("score", team.Score);
        command.Parameters.AddWithValue("team_id", team.Id);
        await command.ExecuteNonQueryAsync();

        dbConnection.Close();
    }
    public async Task CreatePlayersAsync(List<List<Player>> players)
    {
        SQLiteConnection dbConnection = new SQLiteConnection(_connectionString);
        dbConnection.Open();

        string sql = "INSERT INTO player(player_name, team_id) VALUES";
        for (int i = 0; i < players.Count; i++)
        {
            for (int j = 0; j < players[i].Count; j++)
            {
                if ((i == players.Count - 1) && (j == players[j].Count - 1))
                    sql += $"(@player_name_{i}_{j}, @team_id_{i}_{j})";
                else
                    sql += $"(@player_name_{i}_{j}, @team_id_{i}_{j}),";
            }
        }
        SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
        for (int i = 0; i < players.Count; i++)
        {
            for (int j = 0; j < players[i].Count; j++)
            {
                command.Parameters.AddWithValue($"player_name_{i}_{j}", players[i][j].Name);
                command.Parameters.AddWithValue($"team_id_{i}_{j}", players[i][j].Team_id);
            }
        }
        await command.ExecuteNonQueryAsync();

        dbConnection.Close();
    }
    public async Task<List<Team>> GetTeamsSortedbyScoreAsync()
    {
        List<Team> teams = new List<Team>();

        SQLiteConnection dbConnection = new SQLiteConnection(_connectionString);
        dbConnection.Open();

        string sql = "SELECT * FROM team ORDER BY score DESC";
        SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (await reader.ReadAsync())
        {
            teams.Add(new Team
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Score = reader.GetInt32(2)
            });
        }
        reader.Close();

        dbConnection.Close();

        return teams;
    }
    public async Task<List<Player>> GetTeamMembersAsync(int team_id)
    {
        List<Player> players = new List<Player>();

        SQLiteConnection dbConnection = new SQLiteConnection(_connectionString);
        dbConnection.Open();

        string sql = "SELECT * FROM player WHERE team_id = @team_id";
        SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
        command.Parameters.AddWithValue("@team_id", team_id);
        SQLiteDataReader reader = command.ExecuteReader();
        while (await reader.ReadAsync())
        {
            players.Add(new Player
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Team_id = team_id
            });
        }
        reader.Close();

        dbConnection.Close();

        return players;
    }
    public async Task<List<Models.Type>> GetTypesAsync()
    {
        List<Models.Type> types = new List<Models.Type>();

        SQLiteConnection dbConnection = new SQLiteConnection(_connectionString);
        dbConnection.Open();

        string sql = "SELECT * FROM type";
        SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
        SQLiteDataReader reader = command.ExecuteReader();
        while (await reader.ReadAsync())
        {
            types.Add(new Models.Type
            {
                Id = reader.GetInt32(0),
                Category = reader.GetString(1)
            });
        }
        reader.Close();

        dbConnection.Close();

        return types;
    }
}