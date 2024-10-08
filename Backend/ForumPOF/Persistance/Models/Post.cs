using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Models;

public class Post
{
    public Ulid Id { get; set; }
    public Ulid TopicId { get; set; }
    public Ulid? UserId { get; set; }
    public string? Content { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }

    public Topic Topic { get; set; }
    public User? User { get; set; }

    public virtual ICollection<Comment> Comments { get; set; }

    public static Post Create(
        Ulid id,
        Ulid topicId,
        Ulid userId,
        string content,
        DateTime created)
    {
        return new Post()
        {
            Id = id,
            TopicId = topicId,
            UserId = userId,
            Content = content,
            Created = created
        };
    }

    public static Post Update(
        Post post,
        string content,
        DateTime updated
        )
    {
        post.Content = content;
        post.Updated = updated;

        return post;
    }
}
