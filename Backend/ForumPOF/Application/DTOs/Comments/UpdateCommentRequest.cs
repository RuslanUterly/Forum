namespace Application.DTOs.Comments;

public class UpdateCommentRequest
{
    public Ulid Id { get; set; }
    public string Content { get; set; }
}
