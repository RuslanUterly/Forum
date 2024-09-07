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
}
