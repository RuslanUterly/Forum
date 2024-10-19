using Application.DTOs.Comments;
using Application.Helper;
using Application.Interfaces.Auth;
using Mapster;
using Microsoft.AspNetCore.Http;
using Persistance.Models;
using Persistance.Repository.Interfaces;

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
    
    public async Task<Result<Ulid>> Create(string jwt, CommentCreateRequest commentRequest)
    {
        var userId = Reciever.UserUlid(_jwtProvider, jwt);

        if (!await _postRepository.PostExistById(commentRequest.PostId))
            return Result<Ulid>.NotFound("Пост не существует");

        var comment = Comment.Create(Ulid.NewUlid(), commentRequest.PostId, userId, commentRequest.Content, DateTime.Now);

        var isCreated = await _commentRepository.CreateComment(comment);

        return isCreated ?
            Result<Ulid>.Created(comment.Id) :
            Result<Ulid>.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }
    
    public async Task<Result> Update(CommentUpdateRequest commentRequest)
    {
        if (!await _commentRepository.CommentExistById(commentRequest.Id))
            return Result.NotFound("Комментария не существует");

        var comment = await _commentRepository.GetCommentById(commentRequest.Id);

        comment = Comment.Update(comment, commentRequest.Content, DateTime.Now);

        var isUpdated = await _commentRepository.UpdateComment(comment);

        return isUpdated ?
            Result.NoContent() :
            Result.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }

    public async Task<Result> Delete(Ulid id)
    {
        if (!await _commentRepository.CommentExistById(id))
            return Result.NotFound("Комментария не существует");

        var comment = await _commentRepository.GetCommentById(id);

        var isRemoved = await _commentRepository.DeleteComment(comment);

        return isRemoved ?
            Result.NoContent() :
            Result.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }
}
