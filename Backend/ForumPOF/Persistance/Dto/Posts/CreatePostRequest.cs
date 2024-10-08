using Persistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Dto.Posts;

public class CreatePostRequest
{
    public Ulid TopicId { get; set; }
    public string? Content { get; set; }
}
