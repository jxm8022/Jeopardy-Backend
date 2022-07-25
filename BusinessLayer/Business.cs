using DataLayer;
using Models;

namespace BusinessLayer;

public class Business : IBusiness
{
    private readonly IRepository _repo;

    public Business(IRepository repo)
    {
        _repo = repo;
    }

    // Question
    public async Task<List<QA>> GetQuestionsAsync(int subcategory) { return await _repo.GetQuestionsAsync(subcategory); }
    public async Task<int> CreateQuestionAsync(Question question) { return await _repo.CreateQuestionAsync(question); }
    public async Task CreateAnswerAsync(Answer answer) { await _repo.CreateAnswerAsync(answer); }

    // Player
    public async Task<Admin> GetAdminAsync(string username, string password) { return await _repo.GetAdminAsync(username, password); }
    public async Task<List<Player>> GetTeamMembersAsync(int team_id) { return await _repo.GetTeamMembersAsync(team_id); }
    public async Task CreatePlayersAsync(List<List<Player>> players) { await _repo.CreatePlayersAsync(players); }

    // Team
    public async Task<List<Team>> GetTeamsSortedbyScoreAsync() { return await _repo.GetTeamsSortedbyScoreAsync(); }
    public async Task UpdateTeamAsync(Team team) { await _repo.UpdateTeamAsync(team); }
    public async Task CreateTeamsAsync(List<Team> teams) { await _repo.CreateTeamsAsync(teams); }

    // Category
    public async Task<List<Category>> GetCategoriesAsync() { return await _repo.GetCategoriesAsync(); }
    public async Task<List<Subcategory>> GetSubcategoriesAsync(int category_id) { return await _repo.GetSubcategoriesAsync(category_id); }
    public async Task CreateCategoryAsync(string categoryName) { await _repo.CreateCategoryAsync(categoryName); }
    public async Task CreateSubcategoryAsync(Subcategory subcategory) { await _repo.CreateSubcategoryAsync(subcategory); }
}