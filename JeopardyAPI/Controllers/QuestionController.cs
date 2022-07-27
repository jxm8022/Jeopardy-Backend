using Microsoft.AspNetCore.Mvc;
using Models;
using BusinessLayer;

namespace JeopardyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class QuestionController : ControllerBase
{
    private readonly IBusiness _bl;
    private readonly ILogger<QuestionController> _logger;

    public QuestionController(IBusiness bl, ILogger<QuestionController> logger)
    {
        _bl = bl;
        _logger = logger;
    }

    [HttpGet("GetQuestions/{subcategory}")]
    public async Task<ActionResult<List<QA>>> Get(int subcategory)
    {
        List<QA> questions = await _bl.GetQuestionsAsync(subcategory);
        if (questions != null)
        {
            return Ok(questions);
        }
        return NoContent();
    }

    [HttpGet("GetAllQuestions/{subcategory}")]
    public async Task<ActionResult<List<Question>>> GetAll(int subcategory)
    {
        List<Question> questions = await _bl.GetAllQuestionsAsync(subcategory);
        if (questions != null)
        {
            return Ok(questions);
        }
        return NoContent();
    }

    [HttpPost("CreateQuestion")]
    public async Task<ActionResult> PostQuestion(QA question)
    {
        if (question.question.question_entry.Length > 0 && question.question.category_id > 0 && question.answer.answer_entry.Length > 0)
        {
            question.answer.question_id = await _bl.CreateQuestionAsync(question.question);
            Console.WriteLine(question.answer.question_id);
            if (question.answer.question_id != -1)
            {
                await _bl.CreateAnswerAsync(question.answer);
            }
            return Ok();
        }
        return NoContent();
    }
}
