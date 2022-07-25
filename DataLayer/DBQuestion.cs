using System.Data.SqlClient;
using System.Data;
using Models;

namespace DataLayer;

public static class DBQuestion
{
    public static async Task<List<QA>> GetQuestions(int subcategory, string _connectionString)
    {
        return await Task.Factory.StartNew(() =>
        {
            List<QA> questions = new List<QA>();
            DataSet questionSet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("SELECT TOP 5 * FROM Question INNER JOIN Answer ON question.question_id = answer.answer_id WHERE category_id = @category_id ORDER BY RAND()", connection);
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

    public static async Task CreateQuestion(Question question, string _connectionString)
    {
        await Task.Factory.StartNew(() =>
        {
            DataSet questionSet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("SELECT question_entry, category_id FROM Question WHERE question_id = -1", connection);

            SqlDataAdapter questionAdapter = new SqlDataAdapter(cmd);

            questionAdapter.Fill(questionSet, "QuestionTable");

            DataTable? questionTable = questionSet.Tables["QuestionTable"];
            if (questionTable != null)
            {
                DataRow newRow = questionTable.NewRow();
                newRow["question_entry"] = question.question_entry;
                newRow["category_id"] = question.category_id;

                questionTable.Rows.Add(newRow);

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(questionAdapter);
                SqlCommand insert = commandBuilder.GetInsertCommand();

                questionAdapter.InsertCommand = insert;

                questionAdapter.Update(questionTable);
            }
        });
    }
    public static async Task CreateAnswer(Answer answer, string _connectionString)
    {
        await Task.Factory.StartNew(() =>
        {
            DataSet answerSet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("SELECT answer_entry, question_id FROM Answer WHERE answer_id = -1", connection);

            SqlDataAdapter answerAdapter = new SqlDataAdapter(cmd);

            answerAdapter.Fill(answerSet, "AnswerTable");

            DataTable? answerTable = answerSet.Tables["AnswerTable"];
            if (answerTable != null)
            {
                DataRow newRow = answerTable.NewRow();
                newRow["answer_entry"] = answer.answer_entry;
                newRow["question_id"] = answer.question_id;

                answerTable.Rows.Add(newRow);

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(answerAdapter);
                SqlCommand insert = commandBuilder.GetInsertCommand();

                answerAdapter.InsertCommand = insert;

                answerAdapter.Update(answerTable);
            }
        });
    }
}