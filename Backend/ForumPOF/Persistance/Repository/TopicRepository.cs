using Microsoft.EntityFrameworkCore;
using Persistance.Data;
using Persistance.Models;
using Persistance.Repository.Interfaces;

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
        var topicTags = await _context.TopicTags
            .Where(tt => tt.TopicId == topic.Id)
            .Select(tt => tt.Tag)
            .ToArrayAsync();

        // Теги для удаления
        var tagsToRemove = topicTags
            .Where(t => !tags.Contains(t))
            .ToList();

        // Теги для добавления
        var tagsToAdd = tags
            .Where(t => !topicTags.Contains(t))
            .Select(tag => 
                TopicTag.Create(Ulid.NewUlid(), topic.Id, tag.Id)
            );

        if (tagsToRemove.Any())
        {
            var tagsToRemoveIds = tagsToRemove.Select(t => t.Id);
            var topicTagsToRemove = await _context.TopicTags
                .Where(tt => tagsToRemoveIds.Contains(tt.TagId))
                .ToListAsync();

            _context.TopicTags.RemoveRange(topicTagsToRemove);
        }

        if (tagsToAdd.Any())
            await _context.TopicTags.AddRangeAsync(tagsToAdd);

        _context.Update(topic);
        return await Save();
    }

    public async Task<bool> Save() => await _context.SaveChangesAsync() > 0 ? true : false;
}
