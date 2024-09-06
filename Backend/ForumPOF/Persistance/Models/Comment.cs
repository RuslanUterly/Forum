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
}
