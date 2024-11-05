using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Persistance.Repository.Interfaces;

namespace ForumPOF.Filters;

//public class TopicExistFilter(
//    ITopicRepository topicRepository
//    ) : Attribute, IAsyncActionFilter
//{
//    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//    {
//        context.ActionArguments.TryGetValue("topicId", out object? content);

//        if (content is Ulid id)
//        {
//            if (!await topicRepository.TopicExistById(id))
//            {
//                context.Result = new NotFoundObjectResult("Темы не существует");
//                return;
//            }
//        }

//        var result = await next();
//    }
//}
