using Application.DTOs.Categories;
using Application.Services;
using ForumPOF.Attributes;
using ForumPOF.Filters;
using Microsoft.AspNetCore.Mvc;
using Persistance.Enums;

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

    [AuthorizeByRole(UserRole.Admin)]
    [ServiceFilter(typeof(CategoryExistFilter))]
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

    [AuthorizeByRole(UserRole.Admin)]
    [ServiceFilter(typeof(CategoryExistFilter))]
    [HttpPut("{categoryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateCategory(Ulid categoryId, [FromBody] CategoryUpdateRequest categoryRequest)
    {
        var result = await _categoryService.UpdateCategory(categoryId, categoryRequest);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }

    [AuthorizeByRole(UserRole.Admin)]
    [ServiceFilter(typeof(CategoryExistFilter))]
    [HttpDelete("{categoryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteCategory(Ulid categoryId)
    {
        var result = await _categoryService.DeleteCategory(categoryId);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }
}
