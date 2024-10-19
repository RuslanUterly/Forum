using Application.DTOs.Topics;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ForumPOF.Controllers;

[Route("[controller]")]
[ApiController]
public class TopicController(TopicsService topicsService) : Controller
{
    private readonly TopicsService _topicsService = topicsService;

    [HttpGet("all")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<TopicDetailsRequest>))]
    public async Task<IActionResult> GetTopics()
    {
        var topics = await _topicsService.ReceiveAll();

        return Ok(topics);
    }

    [HttpGet("byUser")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<TopicDetailsRequest>))]
    public async Task<IActionResult> GetTopicsByUser()
    {
        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var topics = await _topicsService.ReceiveByUser(jwt);

        return Ok(topics);
    }

    [HttpGet("byTitle/{topicTitle}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<TopicDetailsRequest>))]
    public async Task<IActionResult> GetTopicsByName(string topicTitle)
    {
        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var topics = await _topicsService.ReceiveByName(topicTitle);

        return Ok(topics);
    }

    [HttpGet("{topicId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<TopicDetailsRequest>))]
    public async Task<IActionResult> GetTopicById(Ulid topicId)
    {
        var topic = await _topicsService.ReceiveById(topicId);

        return Ok(topic);
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateTopic([FromQuery] string[] tagTitles, [FromBody] TopicCreateRequest createTopicRequest)
    {
        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var result = await _topicsService.Create(jwt, createTopicRequest, tagTitles);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return CreatedAtAction(nameof(GetTopicById), new { topicId = result.Data}, result.Data);
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateTopic([FromQuery] string[] tagTitles, [FromBody] TopicUpdateRequest updateTopicRequest)
    {
        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var result = await _topicsService.Update(updateTopicRequest, tagTitles);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }

    [HttpDelete("{topicId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteTitle(Ulid topicId)
    {
        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var result = await _topicsService.Delete(topicId);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }
}
