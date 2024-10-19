using Application.DTOs.Posts;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ForumPOF.Controllers;

[Route("[controller]")]
[ApiController]
public class PostController(PostsService postsService) : ControllerBase
{
    private readonly PostsService _postsService = postsService;

    [HttpGet("all")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<PostDetailsRequest>))]
    public async Task<IActionResult> GetPosts()
    {
        var posts = await _postsService.ReceiveAll();

        return Ok(posts);
    }

    [HttpGet("byTopic/{topicId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<PostDetailsRequest>))]
    public async Task<IActionResult> GetPostsByTopic(Ulid topicId)
    {
        var posts = await _postsService.ReceiveByTopic(topicId);

        return Ok(posts);
    }

    [HttpGet("post/{postId}")]
    [ProducesResponseType(200, Type = typeof(PostDetailsRequest))]
    public async Task<IActionResult> GetPostById(Ulid postId)
    {
        var post = await _postsService.ReceiveById(postId);

        return Ok(post);
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreatePost([FromBody] PostCreateRequest postRequest)
    {
        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var result = await _postsService.Create(jwt, postRequest);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return CreatedAtAction(nameof(GetPostById), new { postId = result.Data}, result.Data);
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdatePost([FromBody] PostUpdateRequest postRequest)
    {
        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var result = await _postsService.Update(postRequest);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }

    [HttpDelete("{postId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeletePost(Ulid postId)
    {
        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var result = await _postsService.Delete(postId);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }
}
