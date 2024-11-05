using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Persistance.Repository.Interfaces;

namespace ForumPOF.Filters;

//public class CommentExistFilter(
//    ICommentRepository commentRepository
//    ) : Attribute, IAsyncActionFilter
//{
//    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//    {
//        context.ActionArguments.TryGetValue("commentId", out object? content);

//        if (content is Ulid id)
//        {
//            if (!await commentRepository.CommentExistById(id))
//            {
//                context.Result = new NotFoundObjectResult("Комментария не существует");
//                return;
//            }
//        }

//        var result = await next();
//    }
//}
