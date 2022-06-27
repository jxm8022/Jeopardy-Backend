using Microsoft.AspNetCore.Mvc;
using Models;
using BusinessLayer;

namespace JeopardyAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class CategoryController : ControllerBase
{
    private readonly IBusiness _bl;
    private readonly ILogger<CategoryController> _logger;

    public CategoryController(IBusiness bl, ILogger<CategoryController> logger)
    {
        _bl = bl;
        _logger = logger;
    }

    [HttpGet("GetCategories")]
    public async Task<ActionResult<List<Category>>> Get()
    {
        List<Category> categories = await _bl.GetCategoriesAsync();
        if (categories != null)
        {
            return Ok(categories);
        }
        return NoContent();
    }

    [HttpGet("GetSubCategories/{category_id}")]
    public async Task<ActionResult<List<Subcategory>>> GetSubcategories(int category_id)
    {
        List<Subcategory> subcategories = await _bl.GetSubcategoriesAsync(category_id);
        if (subcategories != null)
        {
            return Ok(subcategories);
        }
        return NoContent();
    }

    [HttpPost("CreateCategory/{category}")]
    public async Task<ActionResult> PostCategory(string categoryName)
    {
        if (categoryName.Length > 0)
        {
            await _bl.CreateCategoryAsync(categoryName);
            return Ok();
        }
        return NoContent();
    }

    [HttpPost("CreateSubcategory/{subcategory}")]
    public async Task<ActionResult> PostSubcategory(Subcategory subcategory)
    {
        if (subcategory.subcategory_name.Length > 0 && subcategory.category_id > 0)
        {
            await _bl.CreateSubcategoryAsync(subcategory);
            return Ok();
        }
        return NoContent();
    }
}
