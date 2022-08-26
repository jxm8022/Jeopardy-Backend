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
    public async Task<ActionResult<List<Models.Type>>> Get()
    {
        List<Models.Type> categories = await _bl.GetCategoriesAsync();
        if (categories != null)
        {
            return Ok(categories);
        }
        return NoContent();
    }

    [HttpPost("CreateCategory/{categoryName}")]
    public async Task<ActionResult> PostCategory(string categoryName)
    {
        if (categoryName.Length > 0)
        {
            await _bl.CreateCategoryAsync(categoryName);
            return Ok();
        }
        return NoContent();
    }

    [HttpPost("CreateSubcategory")]
    public async Task<ActionResult> PostSubcategory(Subcategory subcategory)
    {
        if (subcategory.subcategory_name.Length > 0 && subcategory.category_id > 0)
        {
            await _bl.CreateSubcategoryAsync(subcategory);
            return Ok();
        }
        return NoContent();
    }

    [HttpPut("UpdateCategory")]
    public async Task<ActionResult> UpdateCategory(Category category)
    {
        if (category.category_id > 0 && category.category_name.Length > 0)
        {
            await _bl.UpdateCategoryAsync(category);
            return Ok();
        }
        return NoContent();
    }

    [HttpPut("UpdateSubcategory")]
    public async Task<ActionResult> UpdateSubcategory(Subcategory subcategory)
    {
        if (subcategory.subcategory_id > 0 && subcategory.subcategory_name.Length > 0)
        {
            await _bl.UpdateSubcategoryAsync(subcategory);
            return Ok();
        }
        return NoContent();
    }

    [HttpDelete("DeleteCategory/{category_id}")]
    public async Task<ActionResult> DeleteCategory(int category_id)
    {
        if (category_id > 0)
        {
            await _bl.DeleteCategoryAsync(category_id);
            return Ok();
        }
        return NoContent();
    }

    [HttpDelete("DeleteSubcategory/{subcategory_id}")]
    public async Task<ActionResult> DeleteQuestion(int subcategory_id)
    {
        if (subcategory_id > 0)
        {
            await _bl.DeleteSubcategoryAsync(subcategory_id);
            return Ok();
        }
        return NoContent();
    }
}
