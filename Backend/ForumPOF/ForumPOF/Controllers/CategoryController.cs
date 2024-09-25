using Application.Interfaces.Auth;
using Application.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistance.Dto.Categories;
using Persistance.Dto.Users;
using Persistance.Models;

namespace ForumPOF.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoryController(IMapper mapper, CategoriesService categoryService) : Controller
{
    private readonly IMapper _mapper = mapper;
    private readonly CategoriesService _categoryService = categoryService;

    [HttpGet("recieveAllCategories")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDetailsRequest>))]
    public async Task<IActionResult> GetCategories()
    {
        var categories = _mapper!.Map<List<CategoryDetailsRequest>>(await _categoryService!.RecieveAll());

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

        var categoriy = _mapper!.Map<CategoryDetailsRequest>(await _categoryService!.RecieveByName(categoryName));

        return Ok(categoriy);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateCategory(CategoryRequest categoryRequest)
    {
        if (categoryRequest is null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _categoryService.CreateCategory(categoryRequest.Name);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateCategory(CategoryDetailsRequest categoryRequest)
    {
        if (categoryRequest is null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _categoryService.UpdateCategory(categoryRequest.Id, categoryRequest.Name);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    [HttpDelete]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteCategory(CategoryRequest categoryRequest)
    {
        if (categoryRequest is null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _categoryService.DeleteCategory(categoryRequest.Name);
        
        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }
}
