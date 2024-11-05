using Application.DTOs.Comments;
using Application.Helper;
using ForumPOF.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Persistance.Repository.Interfaces;

namespace ForumPOF.Filters;

//public class PostExistFilter(
//    IPostRepository postRepository
//    ) : Attribute, IAsyncActionFilter
//{
//    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//    {
//        context.ActionArguments.TryGetValue("postId", out object? content);

//        if (content is Ulid id) 
//        {
//            if (!await postRepository.PostExistById(id))
//            {
//                context.Result = new NotFoundObjectResult("Пост не существует");
//                return;
//            }
//        }

//        var result = await next();
//    }
//}