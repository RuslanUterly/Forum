using Application.DTOs.Comments;

namespace Application.DTOs.Posts;

public class PostDetailsRequest
{
    public Ulid Id { get; set; }
    public Ulid TopicId { get; set; }
    public string? Content { get; set; }
    public IEnumerable<CommentDetailsRequest> Comments { get; set; }
}
