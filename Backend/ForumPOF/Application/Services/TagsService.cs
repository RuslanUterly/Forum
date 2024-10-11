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
        if (await _tagRepository.TagExistByTitle(title))
            return Result.Failure("Тэг уже создан");

        var tag = Tag.Create(Ulid.NewUlid(), title);

        var isCreated = await _tagRepository.CreateTag(tag);

        return isCreated ?
            Result.Success("Тэг создан") :
            Result.Failure("Произошла ошибка");
    }

    public async Task<Result> Update(Ulid id, string title)
    {
        if (!await _tagRepository.TagExistById(id))
            return Result.Failure("Тэга не существует");

        var tag = await _tagRepository.GetTagById(id);

        tag = Tag.Update(tag, title);

        var isUpdated = await _tagRepository.UpdateTag(tag);

        return isUpdated ?
            Result.Success("Тэг изменен") :
            Result.Failure("Произошла ошибка");
    }

    public async Task<Result> Delete(string title)
    {
        if (!await _tagRepository.TagExistByTitle(title))
            return Result.Failure("Тэга не существует");

        var tag = await _tagRepository.GetTagByTitle(title);

        var isDeleted = await _tagRepository.DeleteTag(tag);

        return isDeleted ?
            Result.Success("Тэг удален") :
            Result.Failure("Произошла ошибка");
    }
}
