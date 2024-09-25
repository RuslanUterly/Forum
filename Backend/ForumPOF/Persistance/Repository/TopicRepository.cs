using ForumPOF.Contracts.Users;
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

public class TopicRepository(ForumContext context) : ITopicRepository
{
    private readonly ForumContext _context = context;

    public async Task<ICollection<Topic>> GetTopics()
    {
        return await _context.Topics
            .Include(t => t.User)
            .Include(t => t.Posts)
            .Include(t => t.Category)
            .Include(t => t.ThreadTags)
            .AsNoTracking()
            .ToArrayAsync();
    }

    public async Task<ICollection<Topic>> GetTopicsByTitle(string title)
    {
        return await _context.Topics
            .AsNoTracking()
            .Where(t => t.Title.Contains(title))
            .ToArrayAsync();
    }

    public async Task<ICollection<Topic>> GetTopicsByUser(Ulid userId)
    {
        return await _context.Topics
            .AsNoTracking()
            .Where(t => t.UserId == userId)
            .ToArrayAsync();
    }

    public async Task<Topic> GetTopicsById(Ulid id)
    {
        return await _context.Topics 
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<bool> CreateTopic(Tag[] tags, Topic topic)
    {
        var topicTags = tags.Select(tag => 
            TopicTag.Create(Ulid.NewUlid(), topic.Id, tag.Id)
        );

        await _context.AddRangeAsync(topicTags);

        _context.Add(topic);
        return await Save();
    }

    public async Task<bool> DeleteTopic(Topic topic)
    {
        _context.Remove(topic);
        return await Save();
    }

    public async Task<bool> UpdateTopic(Topic topic)
    {
        _context.Update(topic);
        return await Save();
    }

    public async Task<bool> Save() => await _context.SaveChangesAsync() > 0 ? true : false;
}
