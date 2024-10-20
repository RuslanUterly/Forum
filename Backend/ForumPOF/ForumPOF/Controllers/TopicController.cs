using Application.DTOs.Topics;
using Application.Services;
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

    [HttpGet("byUser")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<TopicDetailsRequest>))]
    public async Task<IActionResult> GetTopicsByUser()
    {
        string jwt = Request.Cookies["tasty-cookies"];

        var topics = await _topicsService.ReceiveByUser(jwt);

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
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateTopic([FromQuery] string[] tagTitles, [FromBody] TopicCreateRequest createTopicRequest)
    {
        string jwt = Request.Cookies["tasty-cookies"];

        var result = await _topicsService.Create(jwt, createTopicRequest, tagTitles);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return CreatedAtAction(nameof(GetTopicById), new { topicId = result.Data}, result.Data);
    }

    [Authorize]
    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateTopic([FromQuery] string[] tagTitles, [FromBody] TopicUpdateRequest updateTopicRequest)
    {
        string jwt = Request.Cookies["tasty-cookies"];

        var result = await _topicsService.Update(updateTopicRequest, tagTitles);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{topicId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteTitle(Ulid topicId)
    {
        string jwt = Request.Cookies["tasty-cookies"];

        var result = await _topicsService.Delete(topicId);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }
}
