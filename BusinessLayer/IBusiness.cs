using Models;

namespace BusinessLayer;
public interface IBusiness
{
    // Question
    Task<List<QA>> GetQuestionsAsync(int subcategory);
    Task CreateQuestionAsync(Question question);
    Task CreateAnswerAsync(Answer answer);

    // Player
    Task<Admin> GetAdminAsync(string username, string password);
    Task<List<Player>> GetTeamMembersAsync(int team_id);
    Task CreatePlayersAsync(List<List<Player>> players);

    // Team
    Task<List<Team>> GetTeamsSortedbyScoreAsync();
    Task UpdateTeamAsync(Team team);
    Task CreateTeamsAsync(List<Team> teams);

    // Category
    Task<List<Category>> GetCategoriesAsync();
    Task<List<Subcategory>> GetSubcategoriesAsync(int category_id);
    Task CreateCategoryAsync(string categoryName);
    Task CreateSubcategoryAsync(Subcategory subcategory);
}
