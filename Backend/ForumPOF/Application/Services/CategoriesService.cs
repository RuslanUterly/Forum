using Application.DTOs.Categories;
using Application.Helper;
using Mapster;
using Persistance.Models;
using Persistance.Repository.Interfaces;

namespace Application.Services;

public class CategoriesService(
    ICategoryRepository categoryRepository)
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;

    public async Task<IEnumerable<CategoryDetailsRequest>> RecieveAll()
    {
        var categories = await _categoryRepository.GetCategories();

        return categories.Adapt<IEnumerable<CategoryDetailsRequest>>();
    }

    public async Task<CategoryDetailsRequest> RecieveByName(string categoryName)
    {
        var category = await _categoryRepository.GetCategoryByName(categoryName);

        return category.Adapt<CategoryDetailsRequest>();
    }
   
    public async Task<Result> CreateCategory(CategoryCreateRequest categoryRequest)
    {
        if (await _categoryRepository.CategoryExistByName(categoryRequest.Name))
            return Result.Failure("Категория уже создана");

        //var category = _mapper.Map<Category>(categoryRequest);
        var category = Category.Create(Ulid.NewUlid(), categoryRequest.Name, DateTime.Now);

        var isCreated = await _categoryRepository.CreateCategory(category);

        return isCreated ?
            Result.Success("Категория создана") :
            Result.Failure("Произошла ошибка");
    }
    
    public async Task<Result> UpdateCategory(CategoryUpdateRequest categoryRequest)
    {
        if (!await _categoryRepository.CategoryExistById(categoryRequest.Id))
            return Result.Failure("Категория не найдена");

        if (await _categoryRepository.CategoryExistByName(categoryRequest.Name))
            return Result.Failure("Категория уже создана");

        var category = await _categoryRepository.GetCategoryById(categoryRequest.Id);

        //_mapper.Map(categoryRequest, category);
        category = Category.Update(category, categoryRequest.Name);

        var isUpdated = await _categoryRepository.UpdateCategory(category);

        return isUpdated ?
            Result.Success("Категория изменена!") : 
            Result.Failure("Произошла ошибка!");
    }

    public async Task<Result> DeleteCategory(string categoryName)
    {
        if (!await _categoryRepository.CategoryExistByName(categoryName))
            return Result.Failure("Категория не найдена");

        var category = await _categoryRepository.GetCategoryByName(categoryName);

        var isUpdated = await _categoryRepository.DeleteCategory(category);

        return isUpdated ?
            Result.Success("Категория удалена!") :
            Result.Failure("Произошла ошибка!");
    }
}
