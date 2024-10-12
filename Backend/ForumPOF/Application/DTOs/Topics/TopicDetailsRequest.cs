using Persistance.Models;

namespace Application.DTOs.Topics;

public class TopicDetailsRequest
{
    public Ulid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public User user { get; set; }
    public Category category { get; set; }
    public ICollection<Post> Posts { get; set; }
}

