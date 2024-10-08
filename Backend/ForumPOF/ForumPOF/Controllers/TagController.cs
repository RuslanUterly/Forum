using Application.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Persistance.Dto.Tags;
using Persistance.Dto.Topics;

namespace ForumPOF.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TagController(IMapper mapper, TagsService tagsService) : Controller
{
    private readonly IMapper _mapper = mapper;
    private readonly TagsService _tagsService = tagsService;

    [HttpGet("recieveAllTags")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<TagDetailsRequest>))]
    public async Task<IActionResult> GetTags()
    {
        var tags = _mapper.Map<IEnumerable<TagDetailsRequest>>(await _tagsService.RecieveAll());

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(tags);
    }

    [HttpGet("{tagTitle}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<TagDetailsRequest>))]
    public async Task<IActionResult> GetTagssByTitle(string tagTitle)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var tags = _mapper.Map<IEnumerable<TagDetailsRequest>>(await _tagsService.RecieveByTitle(tagTitle));

        return Ok(tags);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateTag(TagRequest tagRequest)
    {
        if (tagRequest is null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrWhiteSpace(jwt))
                return Unauthorized();

            var result = await _tagsService.Create(tagRequest.Title);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateTag(TagDetailsRequest tagRequest)
    {
        if (tagRequest is null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
                return Unauthorized();

            var result = await _tagsService.Update(tagRequest.Id, tagRequest.Title);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteTitle(TagRequest tagRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
                return Unauthorized();

            var result = await _tagsService.Delete(tagRequest.Title);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
