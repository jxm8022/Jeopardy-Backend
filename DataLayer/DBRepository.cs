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

        string sql = "SELECT question.entry, answer.entry FROM question INNER JOIN answer ON question.question_id = answer.question_id WHERE type_id = @category ORDER BY RANDOM() LIMIT 5";
        SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
        command.Parameters.AddWithValue("@category", category);
        SQLiteDataReader reader = command.ExecuteReader();
        while (await reader.ReadAsync())
        {
            questions.Add(new QA
            {
                Question = new Question
                {
                    Entry = reader.GetString(0)
                },
                Answer = new Answer
                {
                    Entry = reader.GetString(1)
                }
            });
        }
        reader.Close();

        dbConnection.Close();

        return questions;
    }
    public async Task<int> CreateTeamAsync(Team team)
    {
        int team_id = -1;

        SQLiteConnection dbConnection = new SQLiteConnection(_connectionString);
        dbConnection.Open();

        string sql = "INSERT INTO team(team_name) VALUES(@team_name)";
        SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
        command.Parameters.AddWithValue("team_name", team.Name);
        await command.ExecuteNonQueryAsync();
        dbConnection.Close();

        dbConnection.Open();
        sql = "SELECT * FROM team WHERE team_name = @team_name";
        command = new SQLiteCommand(sql, dbConnection);
        command.Parameters.AddWithValue("team_name", team.Name);
        SQLiteDataReader reader = command.ExecuteReader();
        await reader.ReadAsync();
        team_id = reader.GetInt32(0);

        dbConnection.Close();

        return team_id;
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
    public async Task CreatePlayerAsync(Player player)
    {
        SQLiteConnection dbConnection = new SQLiteConnection(_connectionString);
        dbConnection.Open();

        string sql = "INSERT INTO player(player_name, team_id) VALUES(@player_name, @team_id)";
        SQLiteCommand command = new SQLiteCommand(sql, dbConnection);
        command.Parameters.AddWithValue("player_name", player.Name);
        command.Parameters.AddWithValue("team_id", player.Team_id);
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