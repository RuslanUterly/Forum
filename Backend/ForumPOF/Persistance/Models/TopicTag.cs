namespace Persistance.Models;

public class TopicTag
{
    public Ulid TopicTagId { get; set; }
    public Ulid TopicId { get; set; }
    public Ulid TagId { get; set; }

    public Topic Topic { get; set; }
    public Tag Tag { get; set; }

    public static TopicTag Create(
        Ulid id,
        Ulid topicId,
        Ulid tagId)
    {
        return new TopicTag()
        {
            TopicTagId = id,
            TopicId = topicId,
            TagId = tagId,
        };
    }
}