﻿using Application.DTOs.Comments;
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

    [AuthorizeByRole]
    //[ServiceFilter(typeof(PostExistFilter))]
    [HttpPost("{postId}")]
    [ProducesResponseType(201)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateComment(Ulid postId, [FromBody] CommentCreateRequest commentRequest)
    {
        var userId = Ulid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")!.Value);

        var result = await _commentsService.Create(userId, postId, commentRequest);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return CreatedAtAction(nameof(GetCommentById), new { commentId = result.Data }, result.Data);
    }

    [AuthorizeByRole]
    //[ServiceFilter(typeof(CommentExistFilter))]
    [HttpPut("{commentId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateComment(Ulid commentId, [FromBody] CommentUpdateRequest commentRequest)
    {
        var userId = Ulid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")!.Value);
        var userRole = Enum.Parse<UserRole>(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);

        var result = await _commentsService.Update(userId, commentId, userRole, commentRequest);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }

    [AuthorizeByRole]
    //[ServiceFilter(typeof(CommentExistFilter))]
    [HttpDelete("{commentId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteComment(Ulid commentId)
    {
        var userId = Ulid.Parse(User.Claims.FirstOrDefault(c => c.Type == "userId")!.Value);
        var userRole = Enum.Parse<UserRole>(User.FindFirst(ClaimsIdentity.DefaultRoleClaimType)!.Value);

        var result = await _commentsService.Delete(userId, userRole, commentId);

        if (!result)
            return StatusCode(result.StatusCode, result.Error);

        return NoContent();
    }
}
