using Application.DTOs.Comments;
using Application.Services;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Persistance.Models;
using System.ComponentModel.Design;

namespace ForumPOF.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CommentController(IMapper mapper, CommentsService commentsService) : Controller
{
    private readonly IMapper _mapper = mapper;
    private readonly CommentsService _commentsService = commentsService;

    [HttpGet("recieveAll")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CommentDetailsRequest>))]
    public async Task<IActionResult> GetComments()
    {
        var comments = _mapper!.Map<IEnumerable<CommentDetailsRequest>>(await _commentsService.RecieveAll());

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(comments);
    }

    [HttpGet("recieveByPost/{postId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<CommentDetailsRequest>))]
    public async Task<IActionResult> GetCommentsByPost(Ulid postId)
    {
        var comments = _mapper!.Map<IEnumerable<CommentDetailsRequest>>(await _commentsService.RecieveByPost(postId));

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

        var comment = _mapper!.Map<CommentDetailsRequest>(await _commentsService.RecieveCommentById(commentId));

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

        try
        {
            if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
                return Unauthorized();

            var result = await _commentsService.Create(jwt, commentRequest);

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
    public async Task<IActionResult> UpdateComment([FromBody] CommentUpdateRequest commentRequest)
    {
        if (commentRequest is null)
            return BadRequest(ModelState);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
                return Unauthorized();

            var result = await _commentsService.Update(commentRequest);

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
    public async Task<IActionResult> DeleteComment([FromQuery] Ulid commentId)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            if (!Request.Cookies.TryGetValue("tasty-cookies", out string? jwt) || string.IsNullOrEmpty(jwt))
                return Unauthorized();

            var result = await _commentsService.Delete(commentId);

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
