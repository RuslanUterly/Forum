using Application.DTOs.Tags;
using Application.DTOs.Topics;
using Application.Helper;
using Application.Interfaces.Auth;
using Mapster;
using Microsoft.AspNetCore.Http;
using Persistance.Models;
using Persistance.Repository.Interfaces;

namespace Application.Services;

public class TagsService(
    ITagRepository tagRepository,
    IJwtProvider jwtProvider)
{
    private readonly ITagRepository _tagRepository = tagRepository;
    private readonly IJwtProvider _jwtProvider= jwtProvider;

    public async Task<IEnumerable<TagDetailsRequest>> ReceiveAll()
    {
        var tags = await _tagRepository.GetTags();
        return tags.Adapt<IEnumerable<TagDetailsRequest>>();
    }

    public async Task<IEnumerable<TopicDetailsRequest>> ReceiveByTopic(string tagTitle)
    {
        var topic = await _tagRepository.GetTopicsByTag(tagTitle);
        return topic.Adapt<IEnumerable<TopicDetailsRequest>>();
    }

    public async Task<TagDetailsRequest> ReceiveByTitle(string tagTitle)
    {
        var tag = await _tagRepository.GetTagByTitle(tagTitle);
        return tag.Adapt<TagDetailsRequest>();
    }

    public async Task<Result<Ulid>> Create(TagCreateRequest tagRequest)
    {
        if (await _tagRepository.TagExistByTitle(tagRequest.Title))
            return Result<Ulid>.BadRequest("Тэг уже создан");

        var tag = Tag.Create(Ulid.NewUlid(), tagRequest.Title);

        var isCreated = await _tagRepository.CreateTag(tag);

        return isCreated ?
            Result<Ulid>.Created(tag.Id) :
            Result<Ulid>.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }

    public async Task<Result> Update(Ulid tagId, TagUpdateRequest tagRequest)
    {
        if (!await _tagRepository.TagExistById(tagId))
            return Result.NotFound("Тэга не существует");

        var tag = await _tagRepository.GetTagById(tagId);

        tag = Tag.Update(tag, tagRequest.Title);

        var isUpdated = await _tagRepository.UpdateTag(tag);

        return isUpdated ?
            Result.NoContent() :
            Result.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }

    public async Task<Result> Delete(Ulid tagId)
    {
        if (!await _tagRepository.TagExistById(tagId))
            return Result.NotFound("Тэга не существует");

        var tag = await _tagRepository.GetTagById(tagId);

        var isDeleted = await _tagRepository.DeleteTag(tag);

        return isDeleted ?
            Result.NoContent() :
            Result.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }
}
