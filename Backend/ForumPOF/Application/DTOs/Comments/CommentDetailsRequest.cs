namespace Application.DTOs.Comments;

public class CommentDetailsRequest
{
    public Ulid Id { get; set; }
    public Ulid PostId { get; set; }
    public string Content { get; set; }
}
