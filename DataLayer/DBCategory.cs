using System.Data.SqlClient;
using System.Data;
using Models;

namespace DataLayer;

public static class DBCategory
{
    public static async Task<List<Category>> GetCategories(string _connectionString)
    {
        return await Task.Factory.StartNew(() =>
        {
            List<Category> categories = new List<Category>();
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
                    Category category = new Category
                    {
                        category_id = (int)row["category_id"],
                        category_name = (string)row["category_name"]
                    };
                    categories.Add(category);
                }
                return categories;
            }
            return null!;
        });
    }

    public static async Task<List<Subcategory>> GetSubcategories(int category_id, string _connectionString)
    {
        return await Task.Factory.StartNew(() =>
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
        });
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
}