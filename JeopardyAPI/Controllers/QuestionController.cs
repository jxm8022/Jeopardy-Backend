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

    [HttpGet("GetQuestions/{category}")]
    public async Task<ActionResult<List<QA>>> Get(int category)
    {
        List<QA> questions = await _bl.GetQuestionsAsync(category);
        if (questions != null)
        {
            return Ok(questions);
        }
        return NoContent();
    }
}
