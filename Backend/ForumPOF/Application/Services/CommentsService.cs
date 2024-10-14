using Application.DTOs.Comments;
using Application.Helper;
using Application.Interfaces.Auth;
using Mapster;
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

    public async Task<IEnumerable<CommentDetailsRequest>> RecieveAll()
    {
        var comments = await _commentRepository.GetComments();
        return comments.Adapt<IEnumerable<CommentDetailsRequest>>();
    }

    public async Task<IEnumerable<CommentDetailsRequest>> RecieveByPost(Ulid postId)
    {
        var comments = await _commentRepository.GetCommentsByPost(postId);
        return comments.Adapt<IEnumerable<CommentDetailsRequest>>();
    }

    public async Task<CommentDetailsRequest> RecieveCommentById(Ulid id)
    {
        var comment = await _commentRepository.GetCommentById(id);
        return comment.Adapt<CommentDetailsRequest>();
    }
    
    public async Task<Result> Create(string jwt, CommentCreateRequest commentRequest)
    {
        var userId = Reciever.UserUlid(_jwtProvider, jwt);

        if (!await _postRepository.PostExistById(commentRequest.PostId))
            return Result.Failure("Пост не существует");

        //var comment = _mapper.Map<Comment>(commentRequest, opt => opt.Items["userId"] = userId);
        var comment = Comment.Create(Ulid.NewUlid(), commentRequest.PostId, userId, commentRequest.Content, DateTime.Now);

        var isCreated = await _commentRepository.CreateComment(comment);

        return isCreated ?
            Result.Success("Комментарий создан") :
            Result.Failure("Произошла ошибка");
    }
    
    public async Task<Result> Update(CommentUpdateRequest commentRequest)
    {
        if (!await _commentRepository.CommentExistById(commentRequest.Id))
            return Result.Failure("Комментария не существует");

        var comment = await _commentRepository.GetCommentById(commentRequest.Id);

        //_mapper.Map(commentRequest, comment);
        comment = Comment.Update(comment, commentRequest.Content, DateTime.Now);

        var isUpdated = await _commentRepository.UpdateComment(comment);

        return isUpdated ?
            Result.Success("Комментарий обновлен") :
            Result.Failure("Произошла ошибка");
    }

    public async Task<Result> Delete(Ulid id)
    {
        if (!await _commentRepository.CommentExistById(id))
            return Result.Failure("Комментария не существует");

        var comment = await _commentRepository.GetCommentById(id);

        var isRemoved = await _commentRepository.DeleteComment(comment);

        return isRemoved ?
            Result.Success("Комментарий удален") :
            Result.Failure("Произошла ошибка");
    }
}
