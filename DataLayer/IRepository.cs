using Models;

namespace DataLayer;
public interface IRepository
{
    Task<List<QA>> GetQuestionsAsync(int category);
    Task<int> CreateTeamAsync(Team team);
    Task UpdateTeamAsync(Team team);
    Task CreatePlayerAsync(Player player);
    Task<List<Team>> GetTeamsSortedbyScoreAsync();
    Task<List<Player>> GetTeamMembersAsync(int team_id);
    Task<List<Models.Type>> GetTypesAsync();
}
