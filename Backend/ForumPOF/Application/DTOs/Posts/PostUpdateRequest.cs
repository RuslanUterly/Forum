namespace Application.DTOs.Posts;

public class PostUpdateRequest
{
    public Ulid PostId { get; set; }
    public string? Content { get; set; }
}
