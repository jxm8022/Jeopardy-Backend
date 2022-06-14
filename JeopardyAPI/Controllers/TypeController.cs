using Microsoft.AspNetCore.Mvc;
using Models;
using BusinessLayer;

namespace JeopardyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class TypeController : ControllerBase
{
    private readonly IBusiness _bl;
    private readonly ILogger<TypeController> _logger;

    public TypeController(IBusiness bl, ILogger<TypeController> logger)
    {
        _bl = bl;
        _logger = logger;
    }

    [HttpGet("GetTypes")]
    public async Task<ActionResult<List<Models.Type>>> Get()
    {
        List<Models.Type> types = await _bl.GetTypesAsync();
        if (types != null)
        {
            return Ok(types);
        }
        return NoContent();
    }
}
