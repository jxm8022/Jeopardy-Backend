using System.Data.SqlClient;
using System.Data;
using Models;

namespace DataLayer;

public static class DBCategory
{
    public static async Task<List<Models.Type>> GetCategories(string _connectionString)
    {
        return await Task.Factory.StartNew(() =>
        {
            List<Models.Type> categories = new List<Models.Type>();
            DataSet categorySet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("SELECT * FROM Category", connection);

            SqlDataAdapter categoryAdapter = new SqlDataAdapter(cmd);

            categoryAdapter.Fill(categorySet, "CategoryTable");

            DataTable? categoryTable = categorySet.Tables["CategoryTable"];
            if (categoryTable != null && categoryTable.Rows.Count > 0)
            {
                foreach (DataRow row in categoryTable.Rows)
                {
                    Models.Type category = new Models.Type();
                    category.category = new Category
                    {
                        category_id = (int)row["category_id"],
                        category_name = (string)row["category_name"]
                    };
                    category.subcategories = GetSubcategories(category.category.category_id, _connectionString);
                    categories.Add(category);
                }
                return categories;
            }
            return null!;
        });
    }

    private static List<Subcategory> GetSubcategories(int category_id, string _connectionString)
    {
        List<Subcategory> subcategories = new List<Subcategory>();
        DataSet categorySet = new DataSet();

        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand("SELECT * FROM Subcategory WHERE category_id = @category_id", connection);
        cmd.Parameters.AddWithValue("@category_id", category_id);

        SqlDataAdapter categoryAdapter = new SqlDataAdapter(cmd);

        categoryAdapter.Fill(categorySet, "SubcategoryTable");

        DataTable? categoryTable = categorySet.Tables["SubcategoryTable"];
        if (categoryTable != null && categoryTable.Rows.Count > 0)
        {
            foreach (DataRow row in categoryTable.Rows)
            {
                Subcategory subcategory = new Subcategory
                {
                    subcategory_id = (int)row["subcategory_id"],
                    subcategory_name = (string)row["subcategory_name"],
                    category_id = (int)row["category_id"]
                };
                subcategories.Add(subcategory);
            }
            return subcategories;
        }
        return null!;
    }

    public static async Task CreateCategory(string categoryName, string _connectionString)
    {
        await Task.Factory.StartNew(() =>
        {
            DataSet category = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("SELECT category_name FROM Category WHERE category_id = -1", connection);

            SqlDataAdapter categoryAdapter = new SqlDataAdapter(cmd);

            categoryAdapter.Fill(category, "CategoryTable");

            DataTable? categoryTable = category.Tables["CategoryTable"];
            if (categoryTable != null)
            {
                DataRow newRow = categoryTable.NewRow();
                newRow["category_name"] = categoryName;

                categoryTable.Rows.Add(newRow);

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(categoryAdapter);
                SqlCommand insert = commandBuilder.GetInsertCommand();

                categoryAdapter.InsertCommand = insert;

                categoryAdapter.Update(categoryTable);
            }
        });
    }

    public static async Task CreateSubcategory(Subcategory subcategory, string _connectionString)
    {
        await Task.Factory.StartNew(() =>
        {
            DataSet category = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("SELECT subcategory_name, category_id FROM Subcategory WHERE subcategory_id = -1", connection);

            SqlDataAdapter categoryAdapter = new SqlDataAdapter(cmd);

            categoryAdapter.Fill(category, "CategoryTable");

            DataTable? categoryTable = category.Tables["CategoryTable"];
            if (categoryTable != null)
            {
                DataRow newRow = categoryTable.NewRow();
                newRow["subcategory_name"] = subcategory.subcategory_name;
                newRow["category_id"] = subcategory.category_id;

                categoryTable.Rows.Add(newRow);

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(categoryAdapter);
                SqlCommand insert = commandBuilder.GetInsertCommand();

                categoryAdapter.InsertCommand = insert;

                categoryAdapter.Update(categoryTable);
            }
        });
    }

    public static async Task UpdateCategory(Category category, string _connectionString)
    {
        await Task.Factory.StartNew(() =>
        {
            DataSet categorySet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("SELECT category_id, category_name FROM Category WHERE category_id = @category_id", connection);
            cmd.Parameters.AddWithValue("@category_id", category.category_id);

            SqlDataAdapter categoryAdapter = new SqlDataAdapter(cmd);

            categoryAdapter.Fill(categorySet, "CategoryTable");

            DataTable? categoryTable = categorySet.Tables["CategoryTable"];
            if (categoryTable != null && categoryTable.Rows.Count > 0)
            {
                DataColumn[] dt = new DataColumn[1];
                dt[0] = categoryTable.Columns["category_id"]!;
                categoryTable.PrimaryKey = dt;
                DataRow? gameRow = categoryTable.Rows.Find(category.category_id);
                if (gameRow != null)
                {
                    gameRow["category_name"] = category.category_name;
                }

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(categoryAdapter);
                SqlCommand updateCmd = commandBuilder.GetUpdateCommand();

                categoryAdapter.UpdateCommand = updateCmd;
                categoryAdapter.Update(categoryTable);
            }
        });
    }
    public static async Task UpdateSubcategory(Subcategory subcategory, string _connectionString)
    {
        await Task.Factory.StartNew(() =>
        {
            DataSet subcategorySet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("SELECT subcategory_id, subcategory_name, category_id FROM Subcategory WHERE subcategory_id = @subcategory_id", connection);
            cmd.Parameters.AddWithValue("@subcategory_id", subcategory.subcategory_id);

            SqlDataAdapter subcategoryAdapter = new SqlDataAdapter(cmd);

            subcategoryAdapter.Fill(subcategorySet, "SubcategoryTable");

            DataTable? subcategoryTable = subcategorySet.Tables["SubcategoryTable"];
            if (subcategoryTable != null && subcategoryTable.Rows.Count > 0)
            {
                DataColumn[] dt = new DataColumn[1];
                dt[0] = subcategoryTable.Columns["subcategory_id"]!;
                subcategoryTable.PrimaryKey = dt;
                DataRow? gameRow = subcategoryTable.Rows.Find(subcategory.subcategory_id);
                if (gameRow != null)
                {
                    gameRow["subcategory_name"] = subcategory.subcategory_name;
                }

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(subcategoryAdapter);
                SqlCommand updateCmd = commandBuilder.GetUpdateCommand();

                subcategoryAdapter.UpdateCommand = updateCmd;
                subcategoryAdapter.Update(subcategoryTable);
            }
        });
    }
    public static async Task DeleteCategory(int category_id, string _connectionString)
    {
        await Task.Factory.StartNew(() =>
        {
            DataSet categorySet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);

            using SqlCommand cmd = new SqlCommand("DELETE FROM Category WHERE category_id = @category_id", connection);
            cmd.Parameters.AddWithValue("@category_id", category_id);

            SqlDataAdapter categoryAdapter = new SqlDataAdapter(cmd);

            categoryAdapter.Fill(categorySet, "CategoryTable");

            DataTable? categoryTable = categorySet.Tables["CategoryTable"];
            if (categoryTable != null)
            {

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(categoryAdapter);
                SqlCommand delete = commandBuilder.GetDeleteCommand();

                categoryAdapter.DeleteCommand = delete;

                categoryAdapter.Update(categoryTable);
            }
        });
    }
    public static async Task DeleteSubcategory(int subcategory_id, string _connectionString)
    {
        await Task.Factory.StartNew(() =>
        {
            DataSet subcategorySet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);

            using SqlCommand cmd = new SqlCommand("DELETE FROM Subcategory WHERE subcategory_id = @subcategory_id", connection);
            cmd.Parameters.AddWithValue("@subcategory_id", subcategory_id);

            SqlDataAdapter subcategoryAdapter = new SqlDataAdapter(cmd);

            subcategoryAdapter.Fill(subcategorySet, "SubcategoryTable");

            DataTable? subcategoryTable = subcategorySet.Tables["SubcategoryTable"];
            if (subcategoryTable != null)
            {

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(subcategoryAdapter);
                SqlCommand delete = commandBuilder.GetDeleteCommand();

                subcategoryAdapter.DeleteCommand = delete;

                subcategoryAdapter.Update(subcategoryTable);
            }
        });
    }
}