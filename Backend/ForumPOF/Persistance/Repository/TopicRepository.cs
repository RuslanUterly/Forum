using Azure;
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

    public async Task<bool> TopicExistById(Ulid id)
    {
        return await _context.Topics
            .AsNoTracking()
            .AnyAsync(t => t.Id == id);
    }

    public async Task<ICollection<Topic>> GetTopics()
    {
        return await _context.Topics
            .Include(t => t.User)
            .Include(t => t.Posts)
            .Include(t => t.Category)
            .Include(t => t.ThreadTags)
                .ThenInclude(tt => tt.Tag)
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
        var topicTags = topic.ThreadTags;
        _context.TopicTags.RemoveRange(topicTags);

        _context.Remove(topic);
        return await Save();
    }

    public async Task<bool> UpdateTopic(Tag[] tags, Topic topic)
    {
        var topicTags = await _context.TopicTags
            .Where(tt => tt.TopicId == topic.Id)
            .Select(tt => tt.Tag)
            .ToArrayAsync();

        //IEnumerable<Tag> newTags;
        //if (topicTags.Length > tags.Length)
        //{
        //    //удаление
        //    newTags = topicTags.Except(tags);
        //    _context.TopicTags.Remove(await _context.TopicTags.FirstOrDefaultAsync(tt => tt.Tag == newTags));
        //}
        //else 
        //{
        //    //добавление
        //    newTags = tags.Except(topicTags);
        //    var newTopicTags = newTags.Select(tag =>
        //        TopicTag.Create(Ulid.NewUlid(), topic.Id, tag.Id)
        //     );

        //    await _context.AddRangeAsync(topicTags);
        //}

        //var currentTagIds = topicTags
        //    .Select(t => t.Id)
        //    .ToHashSet();

        //var newTagIds = tags
        //    .Select(t => t.Id)
        //    .ToHashSet();

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
