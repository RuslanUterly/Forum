using Application.DTOs.Categories;
using Application.DTOs.Posts;
using Application.DTOs.Tags;
using Application.DTOs.Users;
using Persistance.Models;

namespace Application.DTOs.Topics;

public class TopicDetailsRequest
{
    public Ulid Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public UserDetailsRequest User { get; set; }
    public CategoryDetailsRequest Category { get; set; }
    public ICollection<PostDetailsRequest> Posts { get; set; }
    public ICollection<TagDetailsRequest> Tags { get; set; }
}

