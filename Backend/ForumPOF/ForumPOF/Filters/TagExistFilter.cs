using Application.DTOs.Tags;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Persistance.Repository.Interfaces;

namespace ForumPOF.Filters;

public class TagExistFilter(
    ITagRepository tagRepository
    ) : Attribute, IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        context.ActionArguments.TryGetValue("tagId", out object? contentId);
        context.ActionArguments.TryGetValue("tagRequest", out object? contentName);

        if (contentId is Ulid id)
        {
            if (!await tagRepository.TagExistById(id))
            {
                context.Result = new NotFoundObjectResult("Тэг не найден");
                return;
            }
        }

        if (contentName is TagCreateRequest tagCreate)
        {
            if (await tagRepository.TagExistByTitle(tagCreate.Title))
            {
                context.Result = new ConflictObjectResult("Тэг уже создан");
                return;
            }
        }
        else if (contentName is TagUpdateRequest tagUpdate)
        {
            if (await tagRepository.TagExistByTitle(tagUpdate.Title))
            {
                context.Result = new ConflictObjectResult("Тэг уже создан");
                return;
            }
        }

        var result = await next();
    }
}
