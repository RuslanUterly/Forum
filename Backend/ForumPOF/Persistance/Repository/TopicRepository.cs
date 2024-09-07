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

    public async Task<ICollection<Topic>> GetTopicsByUser(User user)
    {
        return await _context.Topics
            .AsNoTracking()
            .Where(t => t.UserId == user.Id)
            .ToArrayAsync();
    }

    public async Task<bool> CreateTopic(Topic topic)
    {
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
