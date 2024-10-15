using Application.DTOs.Comments;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ForumPOF.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentController(CommentsService commentsService) : Controller
{
    private readonly CommentsService _commentsService = commentsService;

    [HttpGet("recieveAll")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CommentDetailsRequest>))]
    public async Task<IActionResult> GetComments()
    {
        var comments = await _commentsService.RecieveAll();

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(comments);
    }

    [HttpGet("recieveByPost/{postId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CommentDetailsRequest>))]
    public async Task<IActionResult> GetCommentsByPost(Ulid postId)
    {
        var comments = await _commentsService.RecieveByPost(postId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(comments);
    }

    [HttpGet("recieve/{commentId}")]
    [ProducesResponseType(200, Type = typeof(CommentDetailsRequest))]
    public async Task<IActionResult> GetCommentById(Ulid commentId)
    {
        if (commentId == default)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var comment = await _commentsService.RecieveCommentById(commentId);

        return Ok(comment);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateComment([FromBody] CommentCreateRequest commentRequest)
    {
        if (commentRequest is null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var result = await _commentsService.Create(jwt, commentRequest);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateComment([FromBody] CommentUpdateRequest commentRequest)
    {
        if (commentRequest is null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var result = await _commentsService.Update(commentRequest);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }

    [HttpDelete("{commentId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteComment(Ulid commentId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var result = await _commentsService.Delete(commentId);

        if (!result.IsSuccess)
            return BadRequest(result.Message);

        return Ok(result.Message);
    }
}
