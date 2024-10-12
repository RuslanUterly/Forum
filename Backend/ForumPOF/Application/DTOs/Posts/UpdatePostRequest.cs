namespace Application.DTOs.Posts;

public class UpdatePostRequest
{
    public Ulid PostId { get; set; }
    public string? Content { get; set; }
}
