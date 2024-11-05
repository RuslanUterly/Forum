using Application.DTOs.Comments;
using Application.Helper;
using Application.Interfaces.Auth;
using Mapster;
using Microsoft.AspNetCore.Http;
using Persistance.Enums;
using Persistance.Models;
using Persistance.Repository.Interfaces;
using System.Data;

namespace Application.Services;

public class CommentsService(
    ICommentRepository commentRepository,
    IPostRepository postRepository,
    IJwtProvider jwtProvider)
{
    private readonly ICommentRepository _commentRepository = commentRepository;
    private readonly IPostRepository _postRepository = postRepository;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<IEnumerable<CommentDetailsRequest>> ReceiveAll()
    {
        var comments = await _commentRepository.GetComments();
        return comments.Adapt<IEnumerable<CommentDetailsRequest>>();
    }

    public async Task<IEnumerable<CommentDetailsRequest>> ReceiveByPost(Ulid postId)
    {
        var comments = await _commentRepository.GetCommentsByPost(postId);
        return comments.Adapt<IEnumerable<CommentDetailsRequest>>();
    }

    public async Task<CommentDetailsRequest> ReceiveCommentById(Ulid id)
    {
        var comment = await _commentRepository.GetCommentById(id);
        return comment.Adapt<CommentDetailsRequest>();
    }
    
    public async Task<Result<Ulid>> Create(Ulid userId, Ulid postId, CommentCreateRequest commentRequest)
    {
        if (!await _postRepository.PostExistById(postId))
            return Result<Ulid>.NotFound("Пост не существует");

        var comment = Comment.Create(Ulid.NewUlid(), postId, userId, commentRequest.Content, DateTime.Now);

        var isCreated = await _commentRepository.CreateComment(comment);

        return isCreated ?
            Result<Ulid>.Created(comment.Id) :
            Result<Ulid>.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }
    
    public async Task<Result> Update(Ulid userId, Ulid commentId, UserRole role, CommentUpdateRequest commentRequest)
    {
        if (!await _commentRepository.CommentExistById(commentId))
            return Result.NotFound("Комментария не существует");

        var comment = await _commentRepository.GetCommentById(commentId);
        if (comment.UserId != userId && role != UserRole.Admin)
            return Result.Fail(403, "У вас нет доступа к данному комментарию");

        comment = Comment.Update(comment, commentRequest.Content, DateTime.Now);

        var isUpdated = await _commentRepository.UpdateComment(comment);

        return isUpdated ?
            Result.NoContent() :
            Result.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }

    public async Task<Result> Delete(Ulid userId, UserRole role, Ulid commentId)
    {
        if (!await _commentRepository.CommentExistById(commentId))
            return Result.NotFound("Комментария не существует");

        var comment = await _commentRepository.GetCommentById(commentId);
        if (comment.UserId != userId && role != UserRole.Admin)
            return Result.Fail(403, "У вас нет доступа к данному комментарию");

        var isRemoved = await _commentRepository.DeleteComment(comment);

        return isRemoved ?
            Result.NoContent() :
            Result.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }
}
