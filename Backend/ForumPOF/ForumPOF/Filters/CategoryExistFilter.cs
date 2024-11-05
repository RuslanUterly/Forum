using Application.DTOs.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Persistance.Repository.Interfaces;

namespace ForumPOF.Filters;

//public class CategoryExistFilter(
//    ICategoryRepository categoryRepository
//    ) : Attribute, IAsyncActionFilter
//{
//    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
//    {
//        context.ActionArguments.TryGetValue("categoryId", out object? contentId);
//        context.ActionArguments.TryGetValue("categoryRequest", out object? contentName);

//        if (contentId is Ulid id)
//        {
//            if (!await categoryRepository.CategoryExistById(id))
//            {
//                context.Result = new NotFoundObjectResult("Категория не найдена");
//                return;
//            }
//        }

//        if (contentName is CategoryCreateRequest categoryCreate)
//        {
//            if (await categoryRepository.CategoryExistByName(categoryCreate.Name))
//            {
//                context.Result = new NotFoundObjectResult("Категория уже создана");
//                return;
//            }
//        }
//        else if (contentName is CategoryUpdateRequest categoryUpdate)
//        {
//            if (await categoryRepository.CategoryExistByName(categoryUpdate.Name))
//            {
//                context.Result = new NotFoundObjectResult("Категория уже создана");
//                return;
//            }
//        }

//        var result = await next();
//    }
//}
