using Application.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Persistance.Repository.Interfaces;

namespace ForumPOF.Filters;

//public class UserExistFilter(
//    IUserRepository userRepository
//    ) : Attribute, IAsyncActionFilter
//{
//    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//    {
//        context.ActionArguments.TryGetValue("userRequest", out object? content);

//        if (content is LoginUserRequest loginUser)
//        {
//            if (!await userRepository.UserExistByEmail(loginUser.Email))
//            {
//                context.Result = new NotFoundObjectResult("Пользователь не найден");
//                return;
//            }
//        }
//        else if (content is ReestablishUserRequest reestablishUser)
//        {
//            if (!await userRepository.UserExistByEmail(reestablishUser.Email))
//            {
//                context.Result = new NotFoundObjectResult("Пользователь не найден");
//                return;
//            }
//        }

//        var result = await next();
//    }
//}
