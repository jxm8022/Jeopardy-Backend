using System.Data.SQLite;
using Models;

namespace DataLayer;

public class DBRepository : IRepository
{
    private readonly string _connectionString;

    public DBRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<List<Question>> GetQuestionsAsync(string category)
    {
        return new List<Question>();
    }
    public async Task<List<Answer>> GetAnswersAsync(int question_id)
    {
        return new List<Answer>();
    }
    public async Task<int> CreateTeamAsync(Team team)
    {
        return -1;
    }
    public async Task UpdateTeamAsync(Team team)
    {

    }
    public async Task CreatePlayerAsync(Player player)
    {

    }
    public async Task<List<Team>> GetTeamsSortedbyScoreAsync()
    {
        return new List<Team>();
    }
    public async Task<List<Player>> GetTeamMembersAsync(int team_id)
    {
        return new List<Player>();
    }
    public async Task<List<Models.Type>> GetTypesAsync()
    {
        return new List<Models.Type>();
    }
}