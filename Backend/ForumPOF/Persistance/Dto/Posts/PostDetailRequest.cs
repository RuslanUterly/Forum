namespace Persistance.Dto.Posts;

public class PostDetailRequest
{
    public Ulid Id { get; set; }
    public Ulid TopicId { get; set; }
    public string? Content { get; set; }
}
