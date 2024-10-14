using Application.DTOs.Categories;
using Application.Services;
using Microsoft.AspNetCore.Mvc;
using Persistance.Models;

namespace ForumPOF.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController(CategoriesService categoryService) : Controller
{
    private readonly CategoriesService _categoryService = categoryService;

    [HttpGet("recieveAllCategories")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDetailsRequest>))]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await _categoryService!.RecieveAll();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(categories);
    }

    [HttpGet("{categoryName}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
    public async Task<IActionResult> GetCategory(string categoryName)
    {
        if (categoryName is null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var categoriy = await _categoryService!.RecieveByName(categoryName);

        return Ok(categoriy);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateCategory(CategoryCreateRequest categoryRequest)
    {
        if (categoryRequest is null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _categoryService.CreateCategory(categoryRequest);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateCategory(CategoryUpdateRequest categoryRequest)
    {
        if (categoryRequest is null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        //var result = await _categoryService.UpdateCategory(categoryRequest.Id, categoryRequest.Name);
        var result = await _categoryService.UpdateCategory(categoryRequest);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    [HttpDelete("{categoryName}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteCategory(string categoryName)
    {
        if (categoryName is null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _categoryService.DeleteCategory(categoryName);
        
        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }
}
