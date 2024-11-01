using Application.DTOs.Topics;
using Application.Services;
using ForumPOF.Filters;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpGet("byUser")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<TopicDetailsRequest>))]
    public async Task<IActionResult> GetTopicsByUser()
    {
        var userId = Ulid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")!.Value);

        var topics = await _topicsService.ReceiveByUser(userId);

        return Ok(topics);
    }

    [HttpGet("byTitle/{topicTitle}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<TopicDetailsRequest>))]
    public async Task<IActionResult> GetTopicsByName(string topicTitle)
    {
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

    [Authorize]
    [ServiceFilter(typeof(CategoryExistFilter))]
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateTopic(Ulid categoryId, [FromQuery] string[] tagTitles, [FromBody] TopicCreateRequest createTopicRequest)
    {
        var userId = Ulid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")!.Value);

        var result = await _topicsService.Create(userId, categoryId, createTopicRequest, tagTitles);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return CreatedAtAction(nameof(GetTopicById), new { topicId = result.Data}, result.Data);
    }

    [Authorize]
    [ServiceFilter(typeof(TopicExistFilter))]
    [ServiceFilter(typeof(CategoryExistFilter))]
    [HttpPut("{topicId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateTopic(Ulid topicId, Ulid categoryId, [FromQuery] string[] tagTitles, [FromBody] TopicUpdateRequest updateTopicRequest)
    {
        var userId = Ulid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")!.Value);

        var result = await _topicsService.Update(userId, topicId, categoryId, updateTopicRequest, tagTitles);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }

    [Authorize]
    [ServiceFilter(typeof(TopicExistFilter))]
    [HttpDelete("{topicId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteTopic(Ulid topicId)
    {
        var userId = Ulid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")!.Value);

        var result = await _topicsService.Delete(userId, topicId);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }
}
