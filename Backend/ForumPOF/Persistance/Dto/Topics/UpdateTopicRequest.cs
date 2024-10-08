namespace Persistance.Dto.Topics;

public class UpdateTopicRequest
{
    public Ulid TopicId { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public string CategoryName { get; set; }
}



