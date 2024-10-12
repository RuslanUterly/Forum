using Application.DTOs.Posts;
using Application.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace ForumPOF.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PostController(IMapper mapper, PostsService postsService) : Controller
{
    private readonly IMapper _mapper = mapper;
    private readonly PostsService _postsService = postsService;

    [HttpGet("recieveAllPosts")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<PostDetailRequest>))]
    public async Task<IActionResult> GetPosts()
    {
        var posts = _mapper!.Map<IEnumerable<PostDetailRequest>>(await _postsService.RecieveAll());

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(posts);
    }

    [HttpGet("topicId")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<PostDetailRequest>))]
    public async Task<IActionResult> GetPostsByTopic([FromQuery] Ulid topicId)
    {
        var posts = _mapper!.Map<IEnumerable<PostDetailRequest>>(await _postsService.RecieveByTopic(topicId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(posts);
    }

    [HttpGet("postId")]
    [ProducesResponseType(200, Type = typeof(PostDetailRequest))]
    public async Task<IActionResult> GetPostById([FromQuery] Ulid postId)
    {
        if (postId == default)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var post = _mapper!.Map<PostDetailRequest>(await _postsService.RecieveById(postId));

        return Ok(post);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest postRequest)
    {
        if (postRequest is null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
                return Unauthorized();

            var result = await _postsService.Create(jwt, postRequest.TopicId, postRequest.Content!);

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
    public async Task<IActionResult> UpdatePost([FromBody] UpdatePostRequest postRequest)
    {
        if (postRequest is null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
                return Unauthorized();

            var result = await _postsService.Update(postRequest.PostId, postRequest.Content!);

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
    public async Task<IActionResult> DeletePost([FromQuery] Ulid postId)
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
