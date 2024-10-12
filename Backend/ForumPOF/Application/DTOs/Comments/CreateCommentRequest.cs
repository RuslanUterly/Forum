namespace Application.DTOs.Comments;

public class CreateCommentRequest
{
    public Ulid PostId { get; set; }
    public string Content { get; set; }
}