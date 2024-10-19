using Application.DTOs.Categories;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ForumPOF.Controllers;

[Route("[controller]")]
[ApiController]
public class CategoryController(CategoriesService categoryService) : ControllerBase
{
    private readonly CategoriesService _categoryService = categoryService;

    [HttpGet("all")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDetailsRequest>))]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categoryService!.ReceiveAll();

        return Ok(categories);
    }

    [HttpGet("byName/{categoryName}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDetailsRequest>))]
    public async Task<IActionResult> GetCategoryByName(string categoryName)
    {
        var categoriy = await _categoryService!.ReceiveByName(categoryName);

        return Ok(categoriy);
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateCategory(CategoryCreateRequest categoryRequest)
    {
        var result = await _categoryService.CreateCategory(categoryRequest);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return CreatedAtAction(nameof(GetCategoryByName), new { categoryName = categoryRequest.Name }, result.Data);
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateCategory(CategoryUpdateRequest categoryRequest)
    {
        var result = await _categoryService.UpdateCategory(categoryRequest);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }

    [HttpDelete("{categoryName}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteCategory(string categoryName)
    {
        var result = await _categoryService.DeleteCategory(categoryName);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }
}
