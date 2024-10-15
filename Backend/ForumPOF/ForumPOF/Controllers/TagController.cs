using Application.DTOs.Tags;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ForumPOF.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagController(TagsService tagsService) : Controller
{
    private readonly TagsService _tagsService = tagsService;

    [HttpGet("recieveAllTags")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<TagDetailsRequest>))]
    public async Task<IActionResult> GetTags()
    {
        var tags = await _tagsService.RecieveAll();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(tags);
    }

    [HttpGet("{tagTitle}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<TagDetailsRequest>))]
    public async Task<IActionResult> GetTagsByTitle(string tagTitle)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var tags = await _tagsService.RecieveByTitle(tagTitle);

        return Ok(tags);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateTag(TagCreateRequest tagRequest)
    {
        if (tagRequest is null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrWhiteSpace(jwt))
            return Unauthorized();

        var result = await _tagsService.Create(tagRequest);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateTag(TagUpdateRequest tagRequest)
    {
        if (tagRequest is null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var result = await _tagsService.Update(tagRequest);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    [HttpDelete("{tagTitle}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteTag(string tagTitle)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var result = await _tagsService.Delete(tagTitle);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }
}
