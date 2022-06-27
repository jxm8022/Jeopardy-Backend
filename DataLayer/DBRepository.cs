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
    public async Task<List<Player>> GetTeamMembersAsync(int team_id) { return await DBPlayer.GetTeamMembers(team_id, _connectionString); }
    public async Task CreatePlayersAsync(List<List<Player>> players) { await DBPlayer.CreatePlayers(players, _connectionString); }

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