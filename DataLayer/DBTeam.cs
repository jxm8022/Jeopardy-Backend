using System.Data.SqlClient;
using System.Data;
using Models;

namespace DataLayer;

public static class DBTeam
{
    public static async Task<List<Team>> GetTeamsSortedbyScore(string _connectionString)
    {
        return await Task.Factory.StartNew(() =>
        {
            List<Team> teams = new List<Team>();
            DataSet teamSet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("SELECT * FROM Team ORDER BY team_score DESC", connection);

            SqlDataAdapter teamAdapter = new SqlDataAdapter(cmd);

            teamAdapter.Fill(teamSet, "TeamTable");

            DataTable? teamTable = teamSet.Tables["TeamTable"];
            if (teamTable != null && teamTable.Rows.Count > 0)
            {
                foreach (DataRow row in teamTable.Rows)
                {
                    Team team = new Team
                    {
                        team_id = (int)row["team_id"],
                        team_name = (string)row["team_name"],
                        team_score = (int)row["team_score"]
                    };
                    teams.Add(team);
                }
                return teams;
            }
            return null!;
        });
    }
    public static async Task UpdateTeams(List<Team> teams, string _connectionString)
    {
        await Task.Factory.StartNew(() =>
        {
            foreach (Team team in teams)
            {
                UpdateTeam(team, _connectionString);
            }
        });
    }
    private static void UpdateTeam(Team team, string _connectionString)
    {
        DataSet teamSet = new DataSet();

        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand("SELECT * FROM Team WHERE team_id = @team_id", connection);
        cmd.Parameters.AddWithValue("@team_id", team.team_id);

        SqlDataAdapter teamAdapter = new SqlDataAdapter(cmd);

        teamAdapter.Fill(teamSet, "TeamTable");

        DataTable? teamTable = teamSet.Tables["TeamTable"];
        if (teamTable != null && teamTable.Rows.Count > 0)
        {
            DataColumn[] dt = new DataColumn[1];
            dt[0] = teamTable.Columns["team_id"]!;
            teamTable.PrimaryKey = dt;
            DataRow? teamRow = teamTable.Rows.Find(team.team_id);
            if (teamRow != null)
            {
                teamRow["team_score"] = team.team_score;
            }

            SqlCommandBuilder commandBuilder = new SqlCommandBuilder(teamAdapter);
            SqlCommand updateCmd = commandBuilder.GetUpdateCommand();

            teamAdapter.UpdateCommand = updateCmd;
            teamAdapter.Update(teamTable);
        }
    }
    public static async Task CreateTeams(List<Team> teams, string _connectionString)
    {
        await Task.Factory.StartNew(() =>
        {
            DataSet teamSet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);

            using SqlCommand cmd = new SqlCommand("SELECT * FROM Team WHERE team_id = -1", connection);

            SqlDataAdapter teamAdapter = new SqlDataAdapter(cmd);

            teamAdapter.Fill(teamSet, "TeamTable");

            DataTable? teamTable = teamSet.Tables["TeamTable"];
            if (teamTable != null)
            {
                foreach (Team team in teams)
                {
                    DataRow newRow = teamTable.NewRow();
                    newRow["team_name"] = team.team_name;
                    newRow["team_score"] = team.team_score;

                    teamTable.Rows.Add(newRow);
                }

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(teamAdapter);
                SqlCommand insert = commandBuilder.GetInsertCommand();

                teamAdapter.InsertCommand = insert;

                teamAdapter.Update(teamTable);
            }
        });
    }
}