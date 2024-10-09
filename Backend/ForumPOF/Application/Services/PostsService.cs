using Application.Helper;
using Application.Interfaces.Auth;
using Persistance.Models;
using Persistance.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class PostsService(
    IPostRepository postRepository,
    ITopicRepository topicRepository,
    IJwtProvider jwtProvider)
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly ITopicRepository _topicRepository = topicRepository;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<IEnumerable<Post>> RecieveAll()
    {
        return await _postRepository.GetPosts();
    }

    public async Task<IEnumerable<Post>> RecieveByTopic(Ulid topicId)
    {
        return await _postRepository.GetPostsByTopic(topicId);
    }

    public async Task<Post> RecieveById(Ulid id)
    {
        return await _postRepository.GetPostById(id);
    }

    public async Task<Result> Create(string jwt, Ulid topicId, string content)
    {
        Ulid userId = Reciever.UserUlid(_jwtProvider, jwt);

        if (!await _topicRepository.TopicExistById(topicId))
            return Result.Failure("Тема для поста не существует");

        var post = Post.Create(Ulid.NewUlid(), topicId, userId, content, DateTime.Now);
        
        var isCreated = await _postRepository.CreatePost(post);

        return isCreated ?
            Result.Success("Пост создан") :
            Result.Failure("Произошла ошибка");
    } 

    public async Task<Result> Update(Ulid id, string content)
    {
        if (!await _postRepository.PostExistById(id))
            return Result.Failure("Тема для поста не существует");
        
        var post = await _postRepository.GetPostById(id);

        post = Post.Update(post, content, DateTime.Now);

        var isUpdated = await _postRepository.UpdatePost(post);

        return isUpdated ?
            Result.Success("Пост обновлен") :
            Result.Failure("Произошла ошибка");
    }

    public async Task<Result> Delete(Ulid id)
    {
        if (!await _postRepository.PostExistById(id))
            return Result.Failure("Тема для поста не существует");

        var post = await _postRepository.GetPostById(id);

        var isRemoved = await _postRepository.DeletePost(post);

        return isRemoved ?
            Result.Success("Пост удален") :
            Result.Failure("Произошла ошибка");
    }
}
