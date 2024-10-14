using Application.DTOs.Tags;
using Application.DTOs.Topics;
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

public class TagsService(
    ITagRepository tagRepository,
    IJwtProvider jwtProvider)
{
    private readonly ITagRepository _tagRepository = tagRepository;
    private readonly IJwtProvider _jwtProvider= jwtProvider;

    public async Task<IEnumerable<TagDetailsRequest>> RecieveAll()
    {
        var tags = await _tagRepository.GetTags();
        return tags.Adapt<IEnumerable<TagDetailsRequest>>();
    }

    public async Task<IEnumerable<TopicDetailsRequest>> RecieveByTopic(string tagTitle)
    {
        var topic = await _tagRepository.GetTopicsByTag(tagTitle);
        return topic.Adapt<IEnumerable<TopicDetailsRequest>>();
    }

    public async Task<TagDetailsRequest> RecieveByTitle(string tagTitle)
    {
        var tag = await _tagRepository.GetTagByTitle(tagTitle);
        return tag.Adapt<TagDetailsRequest>();
    }

    public async Task<Result> Create(TagCreateRequest tagRequest)
    {
        if (await _tagRepository.TagExistByTitle(tagRequest.Title))
            return Result.Failure("Тэг уже создан");

        var tag = Tag.Create(Ulid.NewUlid(), tagRequest.Title);

        var isCreated = await _tagRepository.CreateTag(tag);

        return isCreated ?
            Result.Success("Тэг создан") :
            Result.Failure("Произошла ошибка");
    }

    public async Task<Result> Update(TagUpdateRequest tagRequest)
    {
        if (!await _tagRepository.TagExistById(tagRequest.Id))
            return Result.Failure("Тэга не существует");

        var tag = await _tagRepository.GetTagById(tagRequest.Id);

        tag = Tag.Update(tag, tagRequest.Title);

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
