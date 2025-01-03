﻿using Application.DTOs.Tags;
using Application.Services;
using ForumPOF.Attributes;
using ForumPOF.Filters;
using Microsoft.AspNetCore.Authorization;
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

    [AuthorizeByRole]
    //[ServiceFilter(typeof(TagExistFilter))]
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateTag(TagCreateRequest tagRequest)
    {
        var result = await _tagsService.Create(tagRequest);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return CreatedAtAction(nameof(GetTagsByTitle), new { tagTitle = tagRequest.Title}, result.Data);
    }

    [AuthorizeByRole]
    //[ServiceFilter(typeof(TagExistFilter))]
    [HttpPut("{tagId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateTag(Ulid tagId, TagUpdateRequest tagRequest)
    {
        var result = await _tagsService.Update(tagId, tagRequest);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }

    [AuthorizeByRole]
    //[ServiceFilter(typeof(TagExistFilter))]
    [HttpDelete("{tagId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteTag(Ulid tagId)
    {
        var result = await _tagsService.Delete(tagId);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }
}
