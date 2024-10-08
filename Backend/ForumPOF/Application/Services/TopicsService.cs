using Application.Helper;
using Application.Interfaces.Auth;
using Azure;
using Microsoft.EntityFrameworkCore.Metadata;
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


    public async Task<IEnumerable<Topic>> RecieveAll()
    {
        return await _topicRepository.GetTopics();
    }

    public async Task<IEnumerable<Topic>> RecieveByUser(string jwt)
    {
        Ulid userId = Reciever.UserUlid(_jwtProvider, jwt);

        return await _topicRepository.GetTopicsByUser(userId);
    }

    public async Task<IEnumerable<Topic>> RecieveByName(string name)
    {
        return await _topicRepository.GetTopicsByTitle(name);
    }
 
    public async Task<Result> Create(string jwt, string title, string content, string categoryName, params string[] tagTitles)
    {
        Ulid userId = Reciever.UserUlid(_jwtProvider, jwt);
        Ulid categoryId = await Reciever.CategoryUlid(_categoryRepository, categoryName);

        if (categoryId == default)
            return Result.Failure("Категории не существует");

        var tagTasks = tagTitles.Select(_tagRepository.GetTagByTitle);
        var tags = await Task.WhenAll(tagTasks);

        foreach (var tag in tags) 
            if (tag is null)
                return Result.Failure("Добавляемый тэг не существует");

        //добавь проверку на валидность id
        //это значит что если categoryId неверн код упадет

        //в след раз пиши подробнее я не помню уже че делать

        var topic = Topic.Create(Ulid.NewUlid(), title, content, userId, categoryId, DateTime.Now);

        var isCreated = await _topicRepository.CreateTopic(tags, topic);

        return isCreated? 
            Result.Success("Тема создана") :
            Result.Failure("Произошла ошибка");
    }

    public async Task<Result> Update(string jwt, Ulid topicId, string title, string content, string categoryName, params string[] tagTitles)
    {
        Ulid categoryId = await Reciever.CategoryUlid(_categoryRepository, categoryName);

        if (categoryId == default)
            return Result.Failure("Категории не существует");

        var topic = await _topicRepository.GetTopicsById(topicId);

        if (topic is null)
            return Result.Failure("Темы не существует");

        var tagTasks = tagTitles.Select(_tagRepository.GetTagByTitle);
        var tags = await Task.WhenAll(tagTasks);

        foreach (var tag in tags)
            if (tag is null)
                return Result.Failure("Добавляемый тэг не существует");

        topic = Topic.Update(topic, title, content, categoryId, DateTime.Now);

        var isUpdated = await _topicRepository.UpdateTopic(tags, topic);

        return isUpdated ?
            Result.Success("Тема обновлена") :
            Result.Failure("Произошла ошибка");
    }

    public async Task<Result> Delete(string jwt, Ulid topicId)
    {
        var topic = await _topicRepository.GetTopicsById(topicId);

        if (topic is null)
            return Result.Failure("Темы не существует");

        var isRemoved = await _topicRepository.DeleteTopic(topic);

        return isRemoved ?
            Result.Success("Тема удалена") :
            Result.Failure("Произошла ошибка");
    }
}
