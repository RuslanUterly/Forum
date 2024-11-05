using Application.DTOs.Posts;
using Application.Services;
using ForumPOF.Attributes;
using ForumPOF.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Persistance.Enums;
using System.Security.Claims;

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

    [AuthorizeByRole]
    [ServiceFilter(typeof(TopicExistFilter))]
    [HttpPost("{topicId}")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreatePost(Ulid topicId, [FromBody] PostCreateRequest postRequest)
    {
        var userId = Ulid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")!.Value);

        var result = await _postsService.Create(userId, topicId, postRequest);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return CreatedAtAction(nameof(GetPostById), new { postId = result.Data}, result.Data);
    }

    [AuthorizeByRole]
    [ServiceFilter(typeof(PostExistFilter))]
    [HttpPut("{postId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdatePost(Ulid postId, [FromBody] PostUpdateRequest postRequest)
    {
        var userId = Ulid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")!.Value);
        var userRole = Enum.Parse<UserRole>(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);

        var result = await _postsService.Update(userId, postId, userRole, postRequest);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }

    [AuthorizeByRole]
    [ServiceFilter(typeof(PostExistFilter))]
    [HttpDelete("{postId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeletePost(Ulid postId)
    {
        var userId = Ulid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")!.Value);
        var userRole = Enum.Parse<UserRole>(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);

        var result = await _postsService.Delete(userId, userRole, postId);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }
}
