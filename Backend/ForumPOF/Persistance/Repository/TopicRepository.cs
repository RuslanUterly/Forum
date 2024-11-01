using Microsoft.EntityFrameworkCore;
using Persistance.Data;
using Persistance.Models;
using Persistance.Repository.Interfaces;
using System.Linq;

namespace Persistance.Repository;

public class TopicRepository(ForumContext context) : ITopicRepository
{
    private readonly ForumContext _context = context;

    public async Task<bool> TopicExistById(Ulid id)
    {
        return await _context.Topics
            .AsNoTracking()
            .AnyAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Topic>> GetTopics()
    {
        return await _context.Topics
            .Include(t => t.User)
            .Include(t => t.Posts)
                .ThenInclude(p => p.Comments)
            .Include(t => t.Category)
            .Include(t => t.ThreadTags)
                .ThenInclude(tt => tt.Tag)
            .AsNoTracking()
            .ToArrayAsync();
    }

    public async Task<IEnumerable<Topic>> GetTopicsByTitle(string title)
    {
        return await _context.Topics
            .Include(t => t.User)
            .Include(t => t.Category)
            .Include(t => t.ThreadTags)
                .ThenInclude(tt => tt.Tag)
            .AsNoTracking()
            .Where(t => t.Title.Contains(title))
            .ToArrayAsync();
    }

    public async Task<IEnumerable<Topic>> GetTopicsByUser(Ulid userId)
    {
        return await _context.Topics
            .Include(t => t.Category)
            .Include(t => t.ThreadTags)
                .ThenInclude(tt => tt.Tag)
            .AsNoTracking()
            .Where(t => t.UserId == userId)
            .ToArrayAsync();
    }

    public async Task<Topic> GetTopicsById(Ulid id)
    {
        return await _context.Topics
            .Include(t => t.User)
            .Include(t => t.Posts)
                .ThenInclude(p => p.Comments)
            .Include(t => t.Category)
            .Include(t => t.ThreadTags)
                .ThenInclude(tt => tt.Tag)
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

    public async Task<bool> UpdateTopic(Tag[] tags, Topic topic)
    {
        var topicTags = topic.ThreadTags.Select(tt => tt.Tag);

        var newTags = tags.Select(t => t.Id).ToHashSet();
        var currentTags = topic.ThreadTags.Select(tt => tt.TagId).ToHashSet();

        var tagsToDelete = topicTags.Where(t => !newTags.Contains(t.Id));
        var tagsToAdd = tags.Where(t => !currentTags.Contains(t.Id));

        if (tagsToDelete.Any())
        {
            var removeTopicTags = topic.ThreadTags.Where(tt => tagsToDelete.Select(t => t.Id).Contains(tt.TagId));
            _context.TopicTags.RemoveRange(removeTopicTags);
        }

        if (tagsToAdd.Any())
        {
            var newTopicTags = tagsToAdd.Select(tag => TopicTag.Create(Ulid.NewUlid(), topic.Id, tag.Id));
            _context.TopicTags.AddRange(newTopicTags);
        }

        _context.Update(topic);
        return await Save();
    }

    public async Task<bool> Save() => await _context.SaveChangesAsync() > 0 ? true : false;
}
