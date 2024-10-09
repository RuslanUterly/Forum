using Persistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Dto.Comments;

public class CreateCommentRequest
{
    public Ulid PostId { get; set; }
    public string Content { get; set; }
}