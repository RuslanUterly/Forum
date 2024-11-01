using Application.DTOs.Categories;
using Application.Helper;
using Mapster;
using Microsoft.AspNetCore.Http;
using Persistance.Models;
using Persistance.Repository.Interfaces;

namespace Application.Services;

public class CategoriesService(
    ICategoryRepository categoryRepository)
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public async Task<IEnumerable<CategoryDetailsRequest>> ReceiveAll()
    {
        var categories = await _categoryRepository.GetCategories();
        return categories.Adapt<IEnumerable<CategoryDetailsRequest>>();
    }

    public async Task<CategoryDetailsRequest> ReceiveByName(string categoryName)
    {
        var category = await _categoryRepository.GetCategoryByName(categoryName);
        return category.Adapt<CategoryDetailsRequest>();
    }

    public async Task<Result<Ulid>> CreateCategory(CategoryCreateRequest categoryRequest)
    {
        //if (await _categoryRepository.CategoryExistByName(categoryRequest.Name))
        //    return Result<Ulid>.BadRequest("Категория уже создана");

        var category = Category.Create(Ulid.NewUlid(), categoryRequest.Name, DateTime.Now);

        var isCreated = await _categoryRepository.CreateCategory(category);

        return isCreated ?
            Result<Ulid>.Created(category.Id) :
            Result<Ulid>.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }
    
    public async Task<Result> UpdateCategory(Ulid categoryId, CategoryUpdateRequest categoryRequest)
    {
        //if (!await _categoryRepository.CategoryExistById(categoryId))
        //    return Result.NotFound("Категория не найдена"); 

        //if (await _categoryRepository.CategoryExistByName(categoryRequest.Name))
        //    return Result.BadRequest("Категория уже создана");

        var category = await _categoryRepository.GetCategoryById(categoryId);

        category = Category.Update(category, categoryRequest.Name);

        var isUpdated = await _categoryRepository.UpdateCategory(category);

        return isUpdated ?
            Result.NoContent() :
            Result.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }

    public async Task<Result> DeleteCategory(Ulid categoryId)
    {
        //if (!await _categoryRepository.CategoryExistByName(categoryName))
        //    return Result.NotFound("Категория не найдена");

        //var category = await _categoryRepository.GetCategoryByName(categoryName);
        var category = await _categoryRepository.GetCategoryById(categoryId);

        var isUpdated = await _categoryRepository.DeleteCategory(category);

        return isUpdated ?
            Result.NoContent() :
            Result.Fail(StatusCodes.Status500InternalServerError, "Произошла ошибка");
    }
}
