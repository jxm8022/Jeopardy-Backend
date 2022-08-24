using System.Data.SqlClient;
using System.Data;
using Models;

namespace DataLayer;

public static class DBAdmin
{
    public static async Task<Admin> GetAdmin(string username, string password, string _connectionString)
    {
        return await Task.Factory.StartNew(() =>
        {
            Admin admin = new Admin();
            DataSet adminSet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("SELECT * FROM Administrator WHERE admin_name = @admin_name", connection);
            cmd.Parameters.AddWithValue("@admin_name", username);

            SqlDataAdapter adminAdapter = new SqlDataAdapter(cmd);

            adminAdapter.Fill(adminSet, "AdminTable");

            DataTable? adminTable = adminSet.Tables["AdminTable"];
            if (adminTable != null && adminTable.Rows.Count > 0)
            {
                string dbPassword = (string)adminTable.Rows[0]["admin_password"];
                if (dbPassword == password)
                {
                    return new Admin
                    {
                        admin_id = (int)adminTable.Rows[0]["admin_id"],
                        admin_name = (string)adminTable.Rows[0]["admin_name"],
                        admin_password = (string)adminTable.Rows[0]["admin_password"],
                        admin_access = (int)adminTable.Rows[0]["admin_access"]
                    };
                }
            }

            return null!;
        });
    }
    public static async Task<List<Admin>> GetAllAdmin(string _connectionString)
    {
        return await Task.Factory.StartNew(() =>
        {
            List<Admin> admins = new List<Admin>();
            DataSet adminSet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("SELECT * FROM Administrator", connection);

            SqlDataAdapter adminAdapter = new SqlDataAdapter(cmd);

            adminAdapter.Fill(adminSet, "AdminTable");

            DataTable? adminTable = adminSet.Tables["AdminTable"];
            if (adminTable != null && adminTable.Rows.Count > 0)
            {
                foreach (DataRow row in adminTable.Rows)
                {
                    Admin admin = new Admin
                    {
                        admin_id = (int)row["admin_id"],
                        admin_name = (string)row["admin_name"],
                        admin_password = (string)row["admin_password"],
                        admin_access = (int)row["admin_access"]
                    };
                    admins.Add(admin);
                }
                return admins;
            }
            return null!;
        });
    }
    public static async Task CreateAdmin(Admin admin, string _connectionString)
    {
        await Task.Factory.StartNew(() =>
        {
            DataSet adminSet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("SELECT admin_name, admin_password, admin_access FROM Administrator WHERE admin_id = -1", connection);

            SqlDataAdapter adminAdapter = new SqlDataAdapter(cmd);

            adminAdapter.Fill(adminSet, "AdminTable");

            DataTable? adminTable = adminSet.Tables["AdminTable"];
            if (adminTable != null)
            {
                DataRow newRow = adminTable.NewRow();
                newRow["admin_name"] = admin.admin_name;
                newRow["admin_password"] = admin.admin_password;
                newRow["admin_access"] = admin.admin_access;

                adminTable.Rows.Add(newRow);

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adminAdapter);
                SqlCommand insert = commandBuilder.GetInsertCommand();

                adminAdapter.InsertCommand = insert;

                adminAdapter.Update(adminTable);
            }
        });
    }
    public static async Task UpdateAdmin(Admin admin, string _connectionString)
    {
        await Task.Factory.StartNew(() =>
        {
            DataSet adminSet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("SELECT admin_id, admin_name, admin_password, admin_access FROM Administrator WHERE admin_id = @admin_id", connection);
            cmd.Parameters.AddWithValue("@admin_id", admin.admin_id);

            SqlDataAdapter adminAdapter = new SqlDataAdapter(cmd);

            adminAdapter.Fill(adminSet, "AdminTable");

            DataTable? adminTable = adminSet.Tables["AdminTable"];
            if (adminTable != null && adminTable.Rows.Count > 0)
            {
                DataColumn[] dt = new DataColumn[1];
                dt[0] = adminTable.Columns["admin_id"]!;
                adminTable.PrimaryKey = dt;
                DataRow? gameRow = adminTable.Rows.Find(admin.admin_id);
                if (gameRow != null)
                {
                    gameRow["admin_name"] = admin.admin_name;
                    gameRow["admin_password"] = admin.admin_password;
                    gameRow["admin_access"] = admin.admin_access;
                }

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adminAdapter);
                SqlCommand updateCmd = commandBuilder.GetUpdateCommand();

                adminAdapter.UpdateCommand = updateCmd;
                adminAdapter.Update(adminTable);
            }
        });
    }
    public static async Task DeleteAdmin(int admin_id, string _connectionString)
    {
        await Task.Factory.StartNew(() =>
        {
            DataSet adminSet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);

            using SqlCommand cmd = new SqlCommand("DELETE FROM Administrator WHERE admin_id = @admin_id", connection);
            cmd.Parameters.AddWithValue("@admin_id", admin_id);

            SqlDataAdapter adminAdapter = new SqlDataAdapter(cmd);

            adminAdapter.Fill(adminSet, "AdminTable");

            DataTable? adminTable = adminSet.Tables["AdminTable"];
            if (adminTable != null)
            {

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(adminAdapter);
                SqlCommand delete = commandBuilder.GetDeleteCommand();

                adminAdapter.DeleteCommand = delete;

                adminAdapter.Update(adminTable);
            }
        });
    }
}