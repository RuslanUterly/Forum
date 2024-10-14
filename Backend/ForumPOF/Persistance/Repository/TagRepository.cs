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

public class TagRepository(ForumContext context) : ITagRepository
{
    private readonly ForumContext _context = context;

    public async Task<bool> TagExistByTitle(string title)
    {
        return await _context.Tags
            .AsNoTracking()
            .AnyAsync(t => t.Title == title);
    }

    public async Task<bool> TagExistById(Ulid id)
    {
        return await _context.Tags
            .AsNoTracking()
            .AnyAsync(t => t.Id == id);
    }

    public async Task<Tag> GetTagByTitle(string title)
    {
        return await _context.Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Title == title);
    }

    public async Task<Tag> GetTagById(Ulid id)
    {
        return await _context.Tags
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Tag>> GetTags()
    {
        return await _context.Tags
            .AsNoTracking()
            .ToArrayAsync();
    }

    public async Task<IEnumerable<Topic>> GetTopicsByTag(string title)
    {
        return await _context.TopicTags
            .Where(t => t.Tag.Title == title)
            .Select(t => t.Topic)
            .ToArrayAsync();
    }

    public async Task<bool> CreateTag(Tag tag)
    {
        _context.Add(tag);
        return await Save();
    }

    public async Task<bool> DeleteTag(Tag tag)
    {
        _context.Remove(tag);
        return await Save();
    }

    public async Task<bool> UpdateTag(Tag tag)
    {
        _context.Update(tag);
        return await Save();
    }

    public async Task<bool> Save() => await _context.SaveChangesAsync() > 0 ? true : false;
}
