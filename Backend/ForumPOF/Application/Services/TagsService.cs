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

public class TagsService(
    ITagRepository tagRepository,
    IJwtProvider jwtProvider)
{
    private readonly ITagRepository _tagRepository = tagRepository;
    private readonly IJwtProvider _jwtProvider= jwtProvider;

    public async Task<IEnumerable<Tag>> RecieveAll()
    {
        return await _tagRepository.GetTags();
    }

    public async Task<IEnumerable<Topic>> RecieveByTopic(string tagTitle)
    {
        return await _tagRepository.GetTopicsByTag(tagTitle);
    }

    public async Task<Tag> RecieveByTitle(string tagTitle)
    {
        return await _tagRepository.GetTagByTitle(tagTitle);
    }

    public async Task<Result> Create(string title)
    {
        var isExist = await _tagRepository.TagExistByTitle(title);

        if (isExist)
            return Result.Failure("Тэг уже создан");

        var topic = Tag.Create(Ulid.NewUlid(), title);

        var isCreated = await _tagRepository.CreateTag(topic);

        return isCreated ?
            Result.Success("Тэг создан") :
            Result.Failure("Произошла ошибка");
    }

    public async Task<Result> Update(Ulid id, string title)
    {
        //var isExist = await _tagRepository.TagExistById(id);

        //if (!isExist)
        //    return Result.Failure("Тэга не существует");

        var topic = await _tagRepository.GetTagById(id);

        if (topic is null)
            return Result.Failure("Тэга не существует");

        topic = Tag.Update(topic, title);

        var isUpdated = await _tagRepository.UpdateTag(topic);

        return isUpdated ?
            Result.Success("Тэг изменен") :
            Result.Failure("Произошла ошибка");
    }

    public async Task<Result> Update(string title)
    {
        //var isExist = await _tagRepository.TagExistByTitle(title);

        //if (!isExist)
        //    return Result.Failure("Тэга не существует");

        var topic = await _tagRepository.GetTagByTitle(title);

        if (topic is null)
            return Result.Failure("Тэга не существует");

        var isDeleted = await _tagRepository.DeleteTag(topic);

        return isDeleted ?
            Result.Success("Тэг удален") :
            Result.Failure("Произошла ошибка");
    }
}
