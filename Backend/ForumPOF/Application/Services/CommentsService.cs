using Application.Helper;
using Application.Interfaces.Auth;
using Persistance.Models;
using Persistance.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class CommentsService(
    ICommentRepository commentRepository,
    IPostRepository postRepository,
    IJwtProvider jwtProvider)
{
    private readonly ICommentRepository _commentRepository = commentRepository;
    private readonly IPostRepository _postRepository = postRepository;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<IEnumerable<Comment>> RecieveAll()
    {
        return await _commentRepository.GetComments();
    }

    public async Task<IEnumerable<Comment>> RecieveByPost(Ulid postId)
    {
        return await _commentRepository.GetCommentsByPost(postId);
    }

    public async Task<Comment> RecieveCommentById(Ulid id)
    {
        return await _commentRepository.GetCommentById(id);
    }

    public async Task<Result> Create(string jwt, Ulid postId, string content)
    {
        var userId = Reciever.UserUlid(_jwtProvider, jwt);

        if (!await _postRepository.PostExistById(postId))
            return Result.Failure("Пост не существует");

        var comment = Comment.Create(Ulid.NewUlid(), postId, userId, content, DateTime.Now);
        
        var isCreated = await _commentRepository.CreateComment(comment);

        return isCreated ?
            Result.Success("Комментарий создан") :
            Result.Failure("Произошла ошибка");
    }

    public async Task<Result> Update(Ulid id, string content)
    {
        if (!await _commentRepository.CommentExistById(id))
            return Result.Failure("Комментария не существует");

        var comment = await _commentRepository.GetCommentById(id);

        comment = Comment.Update(comment, content, DateTime.Now);

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
