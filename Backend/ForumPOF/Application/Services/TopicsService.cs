using Application.DTOs.Topics;
using Application.Helper;
using Application.Interfaces.Auth;
using Mapster;
using Microsoft.AspNetCore.Http;
using Persistance.Models;
using Persistance.Repository.Interfaces;

namespace Application.Services;

public class TopicsService(
    ITopicRepository topicRepository,
    ITagRepository tagRepository,
    ICategoryRepository categoryRepository,
    IJwtProvider jwtProvider)
{
    private readonly ITopicRepository _topicRepository = topicRepository;
    private readonly ITagRepository _tagRepository = tagRepository;
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IJwtProvider _jwtProvider = jwtProvider;


    public async Task<IEnumerable<TopicDetailsRequest>> ReceiveAll()
    {
        var topics = await _topicRepository.GetTopics();
        return topics.Adapt<IEnumerable<TopicDetailsRequest>>();
    }

    public async Task<IEnumerable<TopicDetailsRequest>> ReceiveByUser(Ulid userId)
    {
        var topics = await _topicRepository.GetTopicsByUser(userId);

        return topics.Adapt<IEnumerable<TopicDetailsRequest>>();
    }

    public async Task<IEnumerable<TopicDetailsRequest>> ReceiveByName(string name)
    {
        var topics = await _topicRepository.GetTopicsByTitle(name);
        return topics.Adapt<IEnumerable<TopicDetailsRequest>>();
    }

    public async Task<TopicDetailsRequest> ReceiveById(Ulid id)
    {
        var topic = await _topicRepository.GetTopicsById(id);
        return topic.Adapt<TopicDetailsRequest>();
    }
 
    public async Task<Result<Ulid>> Create(Ulid userId, Ulid categoryId, TopicCreateRequest topicRequest, params string[] tagTitles)
    {
        //Ulid categoryId = await Reciever.CategoryUlid(_categoryRepository, topicRequest.CategoryName);

        //if (categoryId == default)
        //    return Result<Ulid>.NotFound("Категории не существует"); 

        var tagTasks = tagTitles.Select(_tagRepository.GetTagByTitle);
        var tags = await Task.WhenAll(tagTasks);

        foreach (var tag in tags) 
            if (tag is null)
                return Result<Ulid>.NotFound("Добавляемый тэг не существует");

        var topic = Topic.Create(Ulid.NewUlid(), topicRequest.Title, topicRequest.Content, userId, categoryId, DateTime.Now);

        var isCreated = await _topicRepository.CreateTopic(tags, topic);

        return isCreated? 
            Result<Ulid>.Created(topic.Id) :
            Result<Ulid>.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }

    public async Task<Result> Update(Ulid userId, Ulid topicId, Ulid categoryId, TopicUpdateRequest topicRequest, params string[] tagTitles)
    {
        //if (!await _topicRepository.TopicExistById(topicId))
        //    return Result.NotFound("Темы не существует");

        //Ulid categoryId = await Reciever.CategoryUlid(_categoryRepository, categoryName);

        //if (categoryId == default)
        //    return Result.NotFound("Категории не существует");

        var tagTasks = tagTitles.Select(_tagRepository.GetTagByTitle);
        var tags = await Task.WhenAll(tagTasks);

        foreach (var tag in tags)
            if (tag is null)
                return Result.NotFound("Добавляемый тэг не существует");

        var topic = await _topicRepository.GetTopicsById(topicId);
        if (topic.UserId != userId)
            return Result.Fail(403, "У вас нет доступа к данной теме");

        topic = Topic.Update(topic, topicRequest.Title, topicRequest.Content, categoryId, DateTime.Now);

        var isUpdated = await _topicRepository.UpdateTopic(tags, topic);

        return isUpdated ?
            Result.NoContent() :
            Result.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }

    public async Task<Result> Delete(Ulid userId, Ulid topicId)
    {
        //if (!await _topicRepository.TopicExistById(topicId))
        //    return Result.NotFound("Темы не существует");

        var topic = await _topicRepository.GetTopicsById(topicId);
        if (topic.UserId != userId)
            return Result.Fail(403, "У вас нет доступа к данной теме");

        var isRemoved = await _topicRepository.DeleteTopic(topic);

        return isRemoved ?
            Result.NoContent() :
            Result.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }
}
