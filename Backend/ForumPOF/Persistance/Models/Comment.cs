using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Models;

public class Comment
{
    public Ulid Id { get; set; }
    public Ulid? UserId { get; set; }
    public Ulid PostId { get; set; }

    public string Content { get; set; }
    public DateTime Created { get; set; }
    public DateTime Updated { get; set; }

    public Post? Post { get; set; }
    public User User { get; set; }

    public static Comment Create(
        Ulid id,
        Ulid postId,
        Ulid userId,
        string content,
        DateTime created) 
    {
        return new Comment()
        {
            Id = id,
            PostId = postId,
            UserId = userId,
            Content = content,
            Created = created
        };
    }

    public static Comment Update(
        Comment comment,
        string content,
        DateTime updated)
    {
        comment.Content = content;
        comment.Updated = updated;

        return comment;
    }
}
