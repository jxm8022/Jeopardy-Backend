using Models;

namespace DataLayer;
public interface IRepository
{
    Task<List<Question>> GetQuestionsAsync(int category);
    Task<List<Answer>> GetAnswersAsync(int question_id);
    Task<int> CreateTeamAsync(Team team);
    Task UpdateTeamAsync(Team team);
    Task CreatePlayerAsync(Player player);
    Task<List<Team>> GetTeamsSortedbyScoreAsync();
    Task<List<Player>> GetTeamMembersAsync(int team_id);
    Task<List<Models.Type>> GetTypesAsync();
}
