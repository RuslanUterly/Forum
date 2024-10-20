using Application.DTOs.Comments;
using Application.Services;
using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateComment([FromBody] CommentCreateRequest commentRequest)
    {
        string jwt = Request.Cookies["tasty-cookies"]!;
        //var userid = Ulid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId").Value);

        var result = await _commentsService.Create(jwt, commentRequest);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return CreatedAtAction(nameof(GetCommentById), new { commentId = result.Data }, result.Data);
    }

    [Authorize]
    [HttpPut]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateComment([FromBody] CommentUpdateRequest commentRequest)
    {
        string jwt = Request.Cookies["tasty-cookies"]!;

        var result = await _commentsService.Update(commentRequest);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{commentId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteComment(Ulid commentId)
    {
        string jwt = Request.Cookies["tasty-cookies"]!;

        var result = await _commentsService.Delete(commentId);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }
}
