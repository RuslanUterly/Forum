﻿using Application.DTOs.Topics;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ForumPOF.Controllers;

[Route("[controller]")]
[ApiController]
public class TopicController(TopicsService topicsService) : Controller
{
    private readonly TopicsService _topicsService = topicsService;

    [HttpGet("recieveAllTopics")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<TopicDetailsRequest>))]
    public async Task<IActionResult> GetTopics()
    {
        var topics = await _topicsService.RecieveAll();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(topics);
    }

    [HttpGet("recieveByUser")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<TopicDetailsRequest>))]
    public async Task<IActionResult> GetTopicsByUser()
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var topics = await _topicsService.RecieveByUser(jwt);

        return Ok(topics);
    }

    [HttpGet("{topicTitle}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<TopicDetailsRequest>))]
    public async Task<IActionResult> GetTopicsByName(string topicTitle)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var topics = await _topicsService.RecieveByName(topicTitle);

        return Ok(topics);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateTopic([FromQuery] string[] tagTitles, [FromBody] TopicCreateRequest createTopicRequest)
    {
        if (createTopicRequest is null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var result = await _topicsService.Create(jwt, createTopicRequest, tagTitles);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateTopic([FromQuery] string[] tagTitles, [FromBody] TopicUpdateRequest updateTopicRequest)
    {
        if (updateTopicRequest is null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var result = await _topicsService.Update(updateTopicRequest, tagTitles);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    [HttpDelete("{topicId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteTitle(Ulid topicId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var result = await _topicsService.Delete(topicId);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }
}
