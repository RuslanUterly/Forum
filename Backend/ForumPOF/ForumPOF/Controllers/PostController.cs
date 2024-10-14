using Application.DTOs.Posts;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ForumPOF.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController(PostsService postsService) : Controller
{
    private readonly PostsService _postsService = postsService;

    [HttpGet("recieveAllPosts")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<PostDetailsRequest>))]
    public async Task<IActionResult> GetPosts()
    {
        var posts = await _postsService.RecieveAll();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(posts);
    }

    [HttpGet("recieveByTopic/{topicId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<PostDetailsRequest>))]
    public async Task<IActionResult> GetPostsByTopic(Ulid topicId)
    {
        var posts = await _postsService.RecieveByTopic(topicId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(posts);
    }

    [HttpGet("recive/{postId}")]
    [ProducesResponseType(200, Type = typeof(PostDetailsRequest))]
    public async Task<IActionResult> GetPostById(Ulid postId)
    {
        if (postId == default)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var post = await _postsService.RecieveById(postId);

        return Ok(post);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreatePost([FromBody] PostCreateRequest postRequest)
    {
        if (postRequest is null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
                return Unauthorized();

            var result = await _postsService.Create(jwt, postRequest);

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
    public async Task<IActionResult> UpdatePost([FromBody] PostUpdateRequest postRequest)
    {
        if (postRequest is null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
                return Unauthorized();

            var result = await _postsService.Update(postRequest);

            if (!result.IsSuccess)
                return BadRequest(result.Message);

            return Ok(result.Message);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{postId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeletePost(Ulid postId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
                return Unauthorized();

            var result = await _postsService.Delete(postId);

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
