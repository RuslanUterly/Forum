using Application.DTOs.Topics;
using Application.Helper;
using Application.Interfaces.Auth;
using Application.Mappings;
using Azure;
using Mapster;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.VisualBasic;
using Persistance.Models;
using Persistance.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


    public async Task<IEnumerable<TopicDetailsRequest>> RecieveAll()
    {
        var topics = await _topicRepository.GetTopics();
        return topics.Adapt<IEnumerable<TopicDetailsRequest>>(MappingConfig.Register(new TypeAdapterConfig()));
    }

    public async Task<IEnumerable<TopicDetailsRequest>> RecieveByUser(string jwt)
    {
        Ulid userId = Reciever.UserUlid(_jwtProvider, jwt);
        var topics = await _topicRepository.GetTopicsByUser(userId);

        return topics.Adapt<IEnumerable<TopicDetailsRequest>>();
    }

    public async Task<IEnumerable<TopicDetailsRequest>> RecieveByName(string name)
    {
        var topics = await _topicRepository.GetTopicsByTitle(name);
        return topics.Adapt<IEnumerable<TopicDetailsRequest>>();
    }
 
    public async Task<Result> Create(string jwt, TopicCreateRequest topicRequest, params string[] tagTitles)
    {
        Ulid userId = Reciever.UserUlid(_jwtProvider, jwt);
        Ulid categoryId = await Reciever.CategoryUlid(_categoryRepository, topicRequest.CategoryName);

        if (categoryId == default)
            return Result.Failure("Категории не существует"); 

        var tagTasks = tagTitles.Select(_tagRepository.GetTagByTitle);
        var tags = await Task.WhenAll(tagTasks);

        foreach (var tag in tags) 
            if (tag is null)
                return Result.Failure("Добавляемый тэг не существует");

        var topic = Topic.Create(Ulid.NewUlid(), topicRequest.Title, topicRequest.Content, userId, categoryId, DateTime.Now);

        var isCreated = await _topicRepository.CreateTopic(tags, topic);

        return isCreated? 
            Result.Success("Тема создана") :
            Result.Failure("Произошла ошибка");
    }

    public async Task<Result> Update(TopicUpdateRequest topicRequest, params string[] tagTitles)
    {
        if (!await _topicRepository.TopicExistById(topicRequest.TopicId))
            return Result.Failure("Темы не существует");

        Ulid categoryId = await Reciever.CategoryUlid(_categoryRepository, topicRequest.CategoryName);

        if (categoryId == default)
            return Result.Failure("Категории не существует");

        var tagTasks = tagTitles.Select(_tagRepository.GetTagByTitle);
        var tags = await Task.WhenAll(tagTasks);

        foreach (var tag in tags)
            if (tag is null)
                return Result.Failure("Добавляемый тэг не существует");

        var topic = await _topicRepository.GetTopicsById(topicRequest.TopicId);

        topic = Topic.Update(topic, topicRequest.Title, topicRequest.Content, categoryId, DateTime.Now);

        var isUpdated = await _topicRepository.UpdateTopic(tags, topic);

        return isUpdated ?
            Result.Success("Тема обновлена") :
            Result.Failure("Произошла ошибка");
    }

    public async Task<Result> Delete(Ulid topicId)
    {
        if (!await _topicRepository.TopicExistById(topicId))
            return Result.Failure("Темы не существует");

        var topic = await _topicRepository.GetTopicsById(topicId);

        var isRemoved = await _topicRepository.DeleteTopic(topic);

        return isRemoved ?
            Result.Success("Тема удалена") :
            Result.Failure("Произошла ошибка");
    }
}
