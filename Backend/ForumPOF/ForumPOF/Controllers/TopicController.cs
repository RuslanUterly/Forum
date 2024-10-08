﻿using Application.Interfaces.Auth;
using Application.Services;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Persistance.Dto.Categories;
using Persistance.Dto.Topics;
using Persistance.Models;

namespace ForumPOF.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TopicController(IMapper mapper, TopicsService topicsService) : Controller
{
    private readonly IMapper _mapper = mapper;
    private readonly TopicsService _topicsService = topicsService;

    [HttpGet("recieveAllTopics")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<TopicDetailsRequest>))]
    public async Task<IActionResult> GetTopics()
    {
        var topics = _mapper.Map<IEnumerable<TopicDetailsRequest>>(await _topicsService.RecieveAll());

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

        try
        {
            if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
                return Unauthorized();

            var topics = _mapper.Map<IEnumerable<TopicDetailsRequest>>(await _topicsService.RecieveByUser(jwt));

            return Ok(topics);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet("{name}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<TopicDetailsRequest>))]
    public async Task<IActionResult> GetTopicsByName(string name)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
                return Unauthorized();

            var topics = _mapper.Map<IEnumerable<TopicDetailsRequest>>(await _topicsService.RecieveByName(name));

            return Ok(topics);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateTopic([FromQuery] string[] tagTitles, [FromBody] CreateTopicRequest createTopicRequest)
    {
        if (createTopicRequest is null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
                return Unauthorized();

            var result = await _topicsService.Create(jwt, createTopicRequest.Title, createTopicRequest.Content, createTopicRequest.CategoryName, tagTitles);

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
    public async Task<IActionResult> UpdateTopic([FromQuery] string[] tagTitles, [FromBody] UpdateTopicRequest updateTopicRequest)
    {
        if (updateTopicRequest is null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
                return Unauthorized();

            var result = await _topicsService.Update(jwt, updateTopicRequest.TopicId, updateTopicRequest.Title, updateTopicRequest.Content, updateTopicRequest.CategoryName, tagTitles);

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
    public async Task<IActionResult> DeleteTitle([FromQuery] Ulid topicId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
                return Unauthorized();

            var result = await _topicsService.Delete(jwt, topicId);

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
