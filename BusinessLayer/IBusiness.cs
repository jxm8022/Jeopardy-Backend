using Models;

namespace BusinessLayer;
public interface IBusiness
{
    Task<List<QA>> GetQuestionsAsync(int category);
    Task<int> CreateTeamAsync(Team team);
    Task UpdateTeamAsync(Team team);
    Task CreatePlayerAsync(Player player);
    Task<List<Team>> GetTeamsSortedbyScoreAsync();
    Task<List<Player>> GetTeamMembersAsync(int team_id);
    Task<List<Models.Type>> GetTypesAsync();
}
