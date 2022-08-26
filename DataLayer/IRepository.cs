using Models;

namespace DataLayer;
public interface IRepository
{
    // Question
    Task<List<List<QA>>> GetQuestionsAsync(List<int> subcategories);
    Task<List<Question>> GetAllQuestionsAsync(int subcategory);
    Task<int> CreateQuestionAsync(Question question);
    Task CreateAnswerAsync(Answer answer);
    Task UpdateQuestionAsync(Question question);
    Task UpdateAnswerAsync(Answer answer);
    Task DeleteQuestionAsync(int question_id);
    Task DeleteAnswerAsync(int answer_id);

    // Player
    Task<List<Player>> GetPlayersAsync();
    Task<List<Player>> GetTeamMembersAsync(int team_id);
    Task CreatePlayersAsync(List<List<Player>> players);

    // Admin
    Task<Admin> GetAdminAsync(string username, string password);
    Task<List<Admin>> GetAllAdminAsync();
    Task CreateAdminAsync(Admin admin);
    Task UpdateAdminAsync(Admin admin);
    Task DeleteAdminAsync(int id);

    // Team
    Task<List<Team>> GetTeamsSortedbyScoreAsync();
    Task UpdateTeamsAsync(List<Team> teams);
    Task CreateTeamsAsync(List<Team> teams);

    // Category
    Task<List<Models.Type>> GetCategoriesAsync();
    Task CreateCategoryAsync(string categoryName);
    Task CreateSubcategoryAsync(Subcategory subcategory);

    // Game
    Task<int> CreateGameAsync(Game game);
    Task CreateGamestateAsync(List<Gamestate> gamestates);
    Task CreateBoardstateAsync(List<Boardstate> boardstates);
    Task<List<GameUI>> GetSavedGamesAsync();
    Task DeleteBoardstatesAsync(int game_id);
    Task DeleteGamestatesAsync(int game_id);
    Task DeleteGameAsync(int game_id);
    Task UpdateGameAsync(Game game);
    Task UpdateBoardstatesAsync(List<Boardstate> boardstates);
}
