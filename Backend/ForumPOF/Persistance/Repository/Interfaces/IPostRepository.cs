using Persistance.Models;

namespace Persistance.Repository.Interfaces;

public interface IPostRepository
{
    Task<bool> PostExistById(Ulid id);

    Task<IEnumerable<Post>> GetPosts();
    Task<IEnumerable<Post>> GetPostsByTopic(Ulid topicId);
    Task<Post> GetPostById(Ulid id);

    Task<bool> CreatePost(Post post);
    Task<bool> UpdatePost(Post post);
    Task<bool> DeletePost(Post post);

    Task<bool> Save();
}