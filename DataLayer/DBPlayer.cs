using System.Data.SqlClient;
using System.Data;
using Models;

namespace DataLayer;

public static class DBPlayer
{
    public static async Task<List<Player>> GetPlayers(string _connectionString)
    {
        return await Task.Factory.StartNew(() =>
        {
            List<Player> players = new List<Player>();
            DataSet playerSet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("SELECT * FROM Player", connection);

            SqlDataAdapter playerAdapter = new SqlDataAdapter(cmd);

            playerAdapter.Fill(playerSet, "PlayerTable");

            DataTable? playerTable = playerSet.Tables["PlayerTable"];
            if (playerTable != null && playerTable.Rows.Count > 0)
            {
                foreach (DataRow row in playerTable.Rows)
                {
                    Player player = new Player
                    {
                        player_id = (int)row["player_id"],
                        player_name = (string)row["player_name"],
                        team_id = (int)row["team_id"]
                    };
                    players.Add(player);
                }
                return players;
            }
            return null!;
        });
    }
    public static async Task<List<Player>> GetTeamMembers(int team_id, string _connectionString)
    {
        return await Task.Factory.StartNew(() =>
        {
            List<Player> players = new List<Player>();
            DataSet playerSet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("SELECT * FROM Player WHERE team_id = @team_id", connection);
            cmd.Parameters.AddWithValue("@team_id", team_id);

            SqlDataAdapter playerAdapter = new SqlDataAdapter(cmd);

            playerAdapter.Fill(playerSet, "PlayerTable");

            DataTable? playerTable = playerSet.Tables["PlayerTable"];
            if (playerTable != null && playerTable.Rows.Count > 0)
            {
                foreach (DataRow row in playerTable.Rows)
                {
                    Player player = new Player
                    {
                        player_id = (int)row["player_id"],
                        player_name = (string)row["player_name"],
                        team_id = (int)row["team_id"]
                    };
                    players.Add(player);
                }
                return players;
            }
            return null!;
        });
    }
    public static async Task CreatePlayers(List<List<Player>> players, string _connectionString)
    {
        await Task.Factory.StartNew(() =>
        {
            DataSet playerSet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);

            using SqlCommand cmd = new SqlCommand("SELECT * FROM Player WHERE player_id = -1", connection);

            SqlDataAdapter playerAdapter = new SqlDataAdapter(cmd);

            playerAdapter.Fill(playerSet, "PlayerTable");

            DataTable? playerTable = playerSet.Tables["PlayerTable"];
            if (playerTable != null)
            {
                foreach (List<Player> team in players)
                {
                    foreach (Player player in team)
                    {
                        DataRow newRow = playerTable.NewRow();
                        newRow["player_name"] = player.player_name;
                        newRow["team_id"] = player.team_id;

                        playerTable.Rows.Add(newRow);
                    }
                }

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(playerAdapter);
                SqlCommand insert = commandBuilder.GetInsertCommand();

                playerAdapter.InsertCommand = insert;

                playerAdapter.Update(playerTable);
            }
        });
    }
}