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
    public async Task<List<QA>> GetQuestionsAsync(int subcategory) { return await DBQuestion.GetQuestionsAsync(subcategory, _connectionString); }

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