using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Models;

public class Topic
{
    public Ulid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public Ulid? UserId { get; set; }
    public Ulid CategoryId { get; set; }

    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }

    public User? User { get; set; }
    public Category Category { get; set; }
    public ICollection<Post> Posts { get; set; }
    public ICollection<TopicTag> ThreadTags { get; set; }

    public static Topic Create(
        Ulid id, 
        string title, 
        string content,
        Ulid userId, 
        Ulid categoryId,
        DateTime created)
    {
        return new Topic()
        {
            Id = id,
            Title = title,
            UserId = userId,
            CategoryId = categoryId,
            Content = content,
            Created = created
        };
    }

    public static Topic Update(
        Topic topic,
        string title,
        string content,
        Ulid categoryId,
        DateTime updated
        )
    {
        topic.Title = title;
        topic.Content = content;
        topic.CategoryId = categoryId;
        topic.Updated = updated;

        return topic;
    }
}
