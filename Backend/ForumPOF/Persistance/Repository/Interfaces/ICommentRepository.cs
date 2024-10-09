using Persistance.Models;

namespace Persistance.Repository.Interfaces;

public interface ICommentRepository
{
    Task<bool> CommentExistById(Ulid id);

    Task<IEnumerable<Comment>> GetComments();
    Task<IEnumerable<Comment>> GetCommentsByPost(Ulid postId);
    Task<Comment> GetCommentById(Ulid id);

    Task<bool> CreateComment(Comment comment);
    Task<bool> UpdateComment(Comment comment);
    Task<bool> DeleteComment(Comment comment);

    Task<bool> Save();
}
