using Persistance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistance.Repository.Interfaces;

public interface IPostRepository
{
    Task<bool> PostExistById(Ulid id);

    Task<IEnumerable<Post>> GetPosts(); 
    Task<Post> GetPostById(Ulid id);

    Task<bool> CreatePost(Post post);
    Task<bool> UpdatePost(Post post);
    Task<bool> DeletePost(Post post);

    Task<bool> Save();
}
