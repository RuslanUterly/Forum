using Application.DTOs.Tags;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ForumPOF.Controllers;

[Route("[controller]")]
[ApiController]
public class TagController(TagsService tagsService) : ControllerBase
{
    private readonly TagsService _tagsService = tagsService;

    [HttpGet("all")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<TagDetailsRequest>))]
    public async Task<IActionResult> GetTags()
    {
        var tags = await _tagsService.ReceiveAll();

        return Ok(tags);
    }

    [HttpGet("byTitle/{tagTitle}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<TagDetailsRequest>))]
    public async Task<IActionResult> GetTagsByTitle(string tagTitle)
    {
        var tags = await _tagsService.ReceiveByTitle(tagTitle);

        return Ok(tags);
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateTag(TagCreateRequest tagRequest)
    {
        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrWhiteSpace(jwt))
            return Unauthorized();

        var result = await _tagsService.Create(tagRequest);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return CreatedAtAction(nameof(GetTagsByTitle), new { tagTitle = tagRequest.Title}, result.Data);
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateTag(TagUpdateRequest tagRequest)
    {
        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var result = await _tagsService.Update(tagRequest);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }

    [HttpDelete("{tagTitle}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteTag(string tagTitle)
    {
        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var result = await _tagsService.Delete(tagTitle);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }
}
