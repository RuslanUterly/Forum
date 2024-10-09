using Microsoft.EntityFrameworkCore;
using Persistance.Data;
using Persistance.Models;
using Persistance.Repository.Interfaces;

namespace Persistance.Repository;

public class CommentRepository(ForumContext context) : ICommentRepository
{
    private readonly ForumContext _context = context;

    public async Task<bool> CommentExistById(Ulid id)
    {
        return await _context.Comments.AnyAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Comment>> GetComments()
    {
        return await _context.Comments
            .AsNoTracking()
            .ToArrayAsync();
    }

    public async Task<IEnumerable<Comment>> GetCommentsByPost(Ulid postId)
    {
        return await _context.Posts
            .Where(p => p.Id == postId)
            .SelectMany(p => p.Comments)
            .AsNoTracking()
            .ToArrayAsync();
    }

    public async Task<Comment> GetCommentById(Ulid id)
    {
        return await _context.Comments
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<bool> CreateComment(Comment comment)
    {
        _context.Add(comment);
        return await Save();
    }

    public async Task<bool> DeleteComment(Comment comment)
    {
        _context.Remove(comment);
        return await Save();
    }

    public async Task<bool> UpdateComment(Comment comment)
    {
        _context.Update(comment);
        return await Save();
    }

    public async Task<bool> Save() => await _context.SaveChangesAsync() > 0 ? true : false;
}
