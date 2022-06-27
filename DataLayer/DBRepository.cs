using System.Data.SqlClient;
using System.Data;
using Models;

namespace DataLayer;

public class DBRepository : IRepository
{
    private readonly string _connectionString;

    public DBRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Question
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

    // Player
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
    public async Task CreatePlayersAsync(List<List<Player>> players)
    {
        SQLiteConnection dbConnection = new SQLiteConnection(_connectionString);
        dbConnection.Open();

        string sql = "INSERT INTO player(player_name, team_id) VALUES";
        for (int i = 0; i < players.Count; i++)
        {
            for (int j = 0; j < players[i].Count; j++)
            {
                if ((i == players.Count - 1) && (j == players[i].Count - 1))
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

    // Team
    public async Task<List<Team>> GetTeamsSortedbyScoreAsync() { return await DBTeam.GetTeamsSortedbyScore(_connectionString); }
    public async Task UpdateTeamAsync(Team team) { await DBTeam.UpdateTeam(team, _connectionString); }
    public async Task CreateTeamsAsync(List<Team> teams) { await DBTeam.CreateTeams(teams, _connectionString); }

    // Category
    public async Task<List<Category>> GetCategoriesAsync() { return await DBCategory.GetCategories(_connectionString); }
    public async Task<List<Subcategory>> GetSubcategoriesAsync(int category_id) { return await DBCategory.GetSubcategories(category_id, _connectionString); }
    public async Task CreateCategoryAsync(string categoryName) { await DBCategory.CreateCategory(categoryName, _connectionString); }
    public async Task CreateSubcategoryAsync(Subcategory subcategory) { await DBCategory.CreateSubcategory(subcategory, _connectionString); }
}