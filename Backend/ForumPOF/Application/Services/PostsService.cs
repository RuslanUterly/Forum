using Application.DTOs.Posts;
using Application.Helper;
using Application.Interfaces.Auth;
using Mapster;
using Microsoft.AspNetCore.Http;
using Persistance.Enums;
using Persistance.Models;
using Persistance.Repository.Interfaces;
using System.Data;

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

    public async Task<Result<Ulid>> Create(Ulid userId, Ulid topicId, PostCreateRequest postRequest)
    {
        if (!await _topicRepository.TopicExistById(topicId))
            return Result<Ulid>.NotFound("Тема для поста не существует");

        var post = Post.Create(Ulid.NewUlid(), topicId, userId, postRequest.Content!, DateTime.Now);
        
        var isCreated = await _postRepository.CreatePost(post);

        return isCreated ?
            Result<Ulid>.Created(post.Id) :
            Result<Ulid>.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }

    public async Task<Result> Update(Ulid userId, Ulid postId, UserRole role, PostUpdateRequest postRequest)
    {
        if (!await _postRepository.PostExistById(postId))
            return Result.NotFound("Темы для поста не существует");

        var post = await _postRepository.GetPostById(postId);
        if (post.UserId != userId && role != UserRole.Admin)
            return Result.Fail(403, "У вас нет доступа к данному посту");

        post = Post.Update(post, postRequest.Content!, DateTime.Now);

        var isUpdated = await _postRepository.UpdatePost(post);

        return isUpdated ?
            Result.NoContent() :
            Result.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }

    public async Task<Result> Delete(Ulid userId, UserRole role, Ulid postId)
    {
        if (!await _postRepository.PostExistById(postId))
            return Result.NotFound("Темы для поста не существует");

        var post = await _postRepository.GetPostById(postId);
        if (post.UserId != userId && role != UserRole.Admin)
            return Result.Fail(403, "У вас нет доступа к данному посту");

        var isRemoved = await _postRepository.DeletePost(post);

        return isRemoved ?
            Result.NoContent() :
            Result.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }
}
