using System.Data.SqlClient;
using System.Data;
using Models;

namespace DataLayer;

public static class DBQuestion
{
    public static async Task<List<List<QA>>> GetQuestions(List<int> subcategories, string _connectionString)
    {
        return await Task.Factory.StartNew(() =>
        {
            List<List<QA>> questions = new List<List<QA>>();

            foreach (int subcategory in subcategories)
            {
                questions.Add(GetQuestionsForSubcategory(subcategory, _connectionString));
            }

            return questions;
        });
    }

    private static List<QA> GetQuestionsForSubcategory(int subcategory, string _connectionString)
    {
        List<QA> questions = new List<QA>();
        DataSet questionSet = new DataSet();

        using SqlConnection connection = new SqlConnection(_connectionString);
        using SqlCommand cmd = new SqlCommand("SELECT TOP 5 * FROM Question INNER JOIN Answer ON question.question_id = answer.question_id WHERE category_id = @category_id ORDER BY NEWID()", connection);
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
                Console.WriteLine(part);
                questions.Add(part);
            }
            return questions;
        }
        return null!;
    }

    public static async Task<List<Question>> GetAllQuestions(int subcategory, string _connectionString)
    {
        return await Task.Factory.StartNew(() =>
        {
            List<Question> questions = new List<Question>();
            DataSet questionSet = new DataSet();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand cmd = new SqlCommand("SELECT * FROM Question WHERE category_id = @category_id", connection);
            cmd.Parameters.AddWithValue("@category_id", subcategory);

            SqlDataAdapter questionAdapter = new SqlDataAdapter(cmd);

            questionAdapter.Fill(questionSet, "QuestionTable");

            DataTable? questionTable = questionSet.Tables["QuestionTable"];
            if (questionTable != null && questionTable.Rows.Count > 0)
            {
                foreach (DataRow row in questionTable.Rows)
                {
                    Question question = new Question
                    {
                        question_id = (int)row["question_id"],
                        question_entry = (string)row["question_entry"],
                        category_id = (int)row["category_id"]
                    };
                    questions.Add(question);
                }
                return questions;
            }
            return null!;
        });
    }

    public static async Task<int> CreateQuestion(Question question, string _connectionString)
    {
        return await Task.Factory.StartNew(() =>
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

            questionSet = new DataSet();

            using SqlCommand cmd2 = new SqlCommand("SELECT * FROM Question ORDER BY question_id DESC", connection);

            questionAdapter = new SqlDataAdapter(cmd2);

            questionAdapter.Fill(questionSet, "QuestionTable");

            questionTable = questionSet.Tables["QuestionTable"];
            if (questionTable != null && questionTable.Rows.Count > 0)
            {
                return (int)questionTable.Rows[0]["question_id"];
            }

            return -1;
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
