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

    public async Task<List<Question>> GetQuestionsAsync(string category)
    {
        return await _repo.GetQuestionsAsync(category);
    }
    public async Task<List<Answer>> GetAnswersAsync(int question_id)
    {
        return await _repo.GetAnswersAsync(question_id);
    }
    public async Task<int> CreateTeamAsync(Team team)
    {
        return await _repo.CreateTeamAsync(team);
    }
    public async Task UpdateTeamAsync(Team team)
    {
        await _repo.UpdateTeamAsync(team);
    }
    public async Task CreatePlayerAsync(Player player)
    {
        await _repo.CreatePlayerAsync(player);
    }
    public async Task<List<Team>> GetTeamsSortedbyScoreAsync()
    {
        return await _repo.GetTeamsSortedbyScoreAsync();
    }
    public async Task<List<Player>> GetTeamMembersAsync(int team_id)
    {
        return await _repo.GetTeamMembersAsync(team_id);
    }
    public async Task<List<Models.Type>> GetTypesAsync()
    {
        return await _repo.GetTypesAsync();
    }
}