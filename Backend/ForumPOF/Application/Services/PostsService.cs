using Application.DTOs.Posts;
using Application.Helper;
using Application.Interfaces.Auth;
using Mapster;
using Microsoft.AspNetCore.Http;
using Persistance.Models;
using Persistance.Repository.Interfaces;

namespace Application.Services;

public class PostsService(
    IPostRepository postRepository,
    ITopicRepository topicRepository,
    IJwtProvider jwtProvider)
{
    private readonly IPostRepository _postRepository = postRepository;
    private readonly ITopicRepository _topicRepository = topicRepository;
    private readonly IJwtProvider _jwtProvider = jwtProvider;

    public async Task<IEnumerable<PostDetailsRequest>> ReceiveAll()
    {
        var posts = await _postRepository.GetPosts();
        return posts.Adapt<IEnumerable<PostDetailsRequest>>();
    }

    public async Task<IEnumerable<PostDetailsRequest>> ReceiveByTopic(Ulid topicId)
    {
        var posts = await _postRepository.GetPostsByTopic(topicId);
        return posts.Adapt<IEnumerable<PostDetailsRequest>>();
    }

    public async Task<PostDetailsRequest> ReceiveById(Ulid id)
    {
        var post = await _postRepository.GetPostById(id);
        return post.Adapt<PostDetailsRequest>();
    }

    public async Task<Result<Ulid>> Create(Ulid userId, PostCreateRequest postRequest)
    {
        if (!await _topicRepository.TopicExistById(postRequest.TopicId))
            return Result<Ulid>.NotFound("Тема для поста не существует");

        var post = Post.Create(Ulid.NewUlid(), postRequest.TopicId, userId, postRequest.Content, DateTime.Now);
        
        var isCreated = await _postRepository.CreatePost(post);

        return isCreated ?
            Result<Ulid>.Created(post.Id) :
            Result<Ulid>.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }

    public async Task<Result> Update(Ulid userId, PostUpdateRequest postRequest)
    {
        if (!await _postRepository.PostExistById(postRequest.PostId))
            return Result.NotFound("Тема для поста не существует");
        
        var post = await _postRepository.GetPostById(postRequest.PostId);
        if (post.UserId != userId)
            return Result.Fail(403, "У вас нет доступа к данному посту");

        post = Post.Update(post, postRequest.Content, DateTime.Now);

        var isUpdated = await _postRepository.UpdatePost(post);

        return isUpdated ?
            Result.NoContent() :
            Result.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }

    public async Task<Result> Delete(Ulid userId, Ulid id)
    {
        if (!await _postRepository.PostExistById(id))
            return Result.NotFound("Тема для поста не существует");

        var post = await _postRepository.GetPostById(id);
        if (post.UserId != userId)
            return Result.Fail(403, "У вас нет доступа к данному посту");

        var isRemoved = await _postRepository.DeletePost(post);

        return isRemoved ?
            Result.NoContent() :
            Result.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }
}
