using Microsoft.EntityFrameworkCore;
using Persistance.Data;
using Persistance.Models;
using Persistance.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repository;

public class PostRepository(ForumContext context) : IPostRepository
{
    private readonly ForumContext _context = context;

    public async Task<bool> PostExistById(Ulid id)
    {
        return await _context.Posts.AnyAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Post>> GetPosts()
    {
        return await _context.Posts
            .Include(p => p.Comments)
            .AsNoTracking()
            .ToArrayAsync();
    }

    public async Task<Post> GetPostById(Ulid id)
    {
        return await _context.Posts
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> CreatePost(Post post)
    {
        _context.Add(post);
        return await Save();
    }

    public async Task<bool> DeletePost(Post post)
    {
        var commentsOnPost = post.Comments;
        _context.Comments.RemoveRange(commentsOnPost);

        _context.Remove(post);
        return await Save();
    }

    public async Task<bool> UpdatePost(Post post)
    {
        _context.Update(post);
        return await Save();
    }

    public async Task<bool> Save() => await _context.SaveChangesAsync() > 0 ? true : false;
}
