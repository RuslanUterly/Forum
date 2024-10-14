using Application.DTOs.Posts;
using Application.Helper;
using Application.Interfaces.Auth;
using Mapster;
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

    public async Task<IEnumerable<PostDetailsRequest>> RecieveAll()
    {
        var posts = await _postRepository.GetPosts();
        return posts.Adapt<IEnumerable<PostDetailsRequest>>();
    }

    public async Task<IEnumerable<PostDetailsRequest>> RecieveByTopic(Ulid topicId)
    {
        var posts = await _postRepository.GetPostsByTopic(topicId);
        return posts.Adapt<IEnumerable<PostDetailsRequest>>();
    }

    public async Task<PostDetailsRequest> RecieveById(Ulid id)
    {
        var post = await _postRepository.GetPostById(id);
        return post.Adapt<PostDetailsRequest>();
    }

    public async Task<Result> Create(string jwt, PostCreateRequest postRequest)
    {
        Ulid userId = Reciever.UserUlid(_jwtProvider, jwt);

        if (!await _topicRepository.TopicExistById(postRequest.TopicId))
            return Result.Failure("Тема для поста не существует");

        var post = Post.Create(Ulid.NewUlid(), postRequest.TopicId, userId, postRequest.Content, DateTime.Now);
        
        var isCreated = await _postRepository.CreatePost(post);

        return isCreated ?
            Result.Success("Пост создан") :
            Result.Failure("Произошла ошибка");
    } 

    public async Task<Result> Update(PostUpdateRequest postRequest)
    {
        if (!await _postRepository.PostExistById(postRequest.PostId))
            return Result.Failure("Тема для поста не существует");
        
        var post = await _postRepository.GetPostById(postRequest.PostId);

        post = Post.Update(post, postRequest.Content, DateTime.Now);

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
