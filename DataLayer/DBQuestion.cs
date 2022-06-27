using System.Data.SqlClient;
using System.Data;
using Models;

namespace DataLayer;

public static class DBQuestion
{
    public static async Task<List<QA>> GetQuestionsAsync(int subcategory, string _connectionString)
    {
        return await Task.Factory.StartNew(() =>
        {
            List<QA> questions = new List<QA>();
            DataSet questionSet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("SELECT * FROM Question INNER JOIN Answer ON question.question_id = answer.answer_id WHERE category_id = @category_id ORDER BY RANDOM() LIMIT 5", connection);
            cmd.Parameters.AddWithValue("@category_id", subcategory);

            SqlDataAdapter questionAdapter = new SqlDataAdapter(cmd);

            questionAdapter.Fill(questionSet, "QuestionTable");

            DataTable? questionTable = questionSet.Tables["QuestionTable"];
            if (questionTable != null && questionTable.Rows.Count > 0)
            {
                foreach (DataRow row in questionTable.Rows)
                {
                    QA part = new QA
                    {
                        question = new Question
                        {
                            question_id = (int)row["question_id"],
                            question_entry = (string)row["question_entry"],
                            category_id = (int)row["category_id"]
                        },
                        answer = new Answer
                        {
                            answer_id = (int)row["answer_id"],
                            answer_entry = (string)row["answer_entry"],
                            question_id = (int)row["question_id"]
                        }
                    };
                    questions.Add(part);
                }
                return questions;
            }
            return null!;
        });
    }
}