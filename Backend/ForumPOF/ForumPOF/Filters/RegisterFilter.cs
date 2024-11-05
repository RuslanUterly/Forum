using Application.DTOs.Users;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Persistance.Repository.Interfaces;

namespace ForumPOF.Filters;

//public class RegisterFilter(
//    IUserRepository userRepository
//    ) : Attribute, IAsyncActionFilter
//{
//    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//    {
//        context.ActionArguments.TryGetValue("userRequest", out object? content);

//        if (content is RegisterUserRequest user)
//        {
//            if (await userRepository.UserExistByEmail(user.Email))
//            {
//                context.Result = new ConflictObjectResult("Пользователь уже зарегистрирован");
//                return;
//            }
            
//            if (await userRepository.UserExistByUsername(user.UserName))
//            {
//                context.Result = new ConflictObjectResult("Имя пользователя занято");
//                return;
//            }
//        }

//        var result = await next();
//    }
//}
