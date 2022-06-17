using Models;

namespace DataLayer;
public interface IRepository
{
    Task<List<QA>> GetQuestionsAsync(int category);
    Task CreateTeamsAsync(List<Team> teams);
    Task UpdateTeamAsync(Team team);
    Task CreatePlayersAsync(List<List<Player>> players);
    Task<List<Team>> GetTeamsSortedbyScoreAsync();
    Task<List<Player>> GetTeamMembersAsync(int team_id);
    Task<List<Models.Type>> GetTypesAsync();
}
