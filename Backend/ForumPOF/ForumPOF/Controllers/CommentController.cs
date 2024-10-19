using Application.DTOs.Comments;
using Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace ForumPOF.Controllers;

[Route("[controller]")]
[ApiController]
public class CommentController(CommentsService commentsService) : ControllerBase
{
    private readonly CommentsService _commentsService = commentsService;

    [HttpGet("all")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CommentDetailsRequest>))]
    public async Task<IActionResult> GetComments()
    {
        var comments = await _commentsService.ReceiveAll();

        return Ok(comments);
    }

    [HttpGet("byPost/{postId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CommentDetailsRequest>))]
    public async Task<IActionResult> GetCommentsByPost(Ulid postId)
    {
        var comments = await _commentsService.ReceiveByPost(postId);

        return Ok(comments);
    }

    [HttpGet("comment/{commentId}")]
    [ProducesResponseType(200, Type = typeof(CommentDetailsRequest))]
    public async Task<IActionResult> GetCommentById(Ulid commentId)
    {
        var comment = await _commentsService.ReceiveCommentById(commentId);

        return Ok(comment);
    }

    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateComment([FromBody] CommentCreateRequest commentRequest)
    {
        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var result = await _commentsService.Create(jwt, commentRequest);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return CreatedAtAction(nameof(GetCommentById), new { commentId = result.Data }, result.Data);
    }

    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateComment([FromBody] CommentUpdateRequest commentRequest)
    {
        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var result = await _commentsService.Update(commentRequest);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }

    [HttpDelete("{commentId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteComment(Ulid commentId)
    {
        if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
            return Unauthorized();

        var result = await _commentsService.Delete(commentId);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }
}
