namespace Application.DTOs.Posts;

public class CreatePostRequest
{
    public Ulid TopicId { get; set; }
    public string? Content { get; set; }
}
