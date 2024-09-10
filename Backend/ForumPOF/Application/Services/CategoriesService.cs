using Application.Helper;
using Persistance.Models;
using Persistance.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class CategoriesService(
    ICategoryRepository categoryRepository)
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;


    public async Task<IEnumerable<Category>> RecieveAll()
    {
        return await _categoryRepository.GetCategories();
    }

    public async Task<Category> RecieveByName(string categoryName)
    {
        return await _categoryRepository.GetCategoryByName(categoryName);
    }

    public async Task<Result> CreateCategory(string categoryName)
    {
        //var isExist = await _categoryRepository.CategoryExistByName(categoryName);

        //if (isExist is not false)
        //    return Result.Failure("Категория уже создана");

        var category = await _categoryRepository.GetCategoryByName(categoryName);
        
        if (category is not null)
            return Result.Failure("Категория уже создана");

        category = Category.Create(Ulid.NewUlid(), categoryName, DateTime.Now);
        
        var isCreated = await _categoryRepository.CreateCategory(category);

        return isCreated ?
            Result.Success("Категория создана") :
            Result.Failure("Произошла ошибка");
    }

    public async Task<Result> UpdateCategory(Ulid ulid, string categoryName)
    {
        //var isExist = await _categoryRepository.CategoryExistById(ulid);

        //if (isExist is not true)
        //    return Result.Failure("Категория не найдена");

        var category = await _categoryRepository.GetCategoryById(ulid);

        if (category is null)
            return Result.Failure("Категория не найдена");

        category = Category.Update(category, categoryName);

        var isUpdated = await _categoryRepository.UpdateCategory(category);

        return isUpdated ?
            Result.Success("Категория изменена!") : 
            Result.Failure("Произошла ошибка!");
    }

    public async Task<Result> DeleteCategory(string categoryName)
    {
        //var isExist = await _categoryRepository.CategoryExistByName(categoryName);
        //if (isExist is false) 
        //    return Result.Failure("Категория не найдена");

        var category = await _categoryRepository.GetCategoryByName(categoryName);

        if (category is null)
            return Result.Failure("Категория не найдена");

        var isUpdated = await _categoryRepository.DeleteCategory(category);

        return isUpdated ?
            Result.Success("Категория удалена!") :
            Result.Failure("Произошла ошибка!");
    }
}
