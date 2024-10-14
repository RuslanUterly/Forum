using Application.DTOs.Topics;
using Persistance.Models;

namespace Application.Mappings;

public static class TopicsMapping
{
    public static IEnumerable<TopicDetailsRequest> ToDetails(this IEnumerable<Topic>? topics)
    {
        return topics.Select(t => new TopicDetailsRequest()
        {
            Id = t.Id,
            Title = t.Title,
            Content = t.Content,
            User = t.User,
            Category = t.Category,
            Posts = t.Posts,
        });
    }
}
