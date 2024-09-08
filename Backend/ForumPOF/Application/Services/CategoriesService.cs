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

    public async Task<string> CreateCategory(string categoryName)
    {
        var category = await _categoryRepository.GetCategoryByName(categoryName);

        if (category is not null)
            throw new Exception("Категория уже создана");

        category = Category.Create(Ulid.NewUlid(), categoryName, DateTime.Now);
        
        return await _categoryRepository.CreateCategory(category) ?
            "Категория создана!" : "Произошла ошибка!";
    }

    public async Task<string> UpdateCategory(Ulid ulid, string categoryName)
    {
        var category = await _categoryRepository.GetCategoryById(ulid);

        if (category is null)
            throw new Exception("Категория не найдена");

        category = Category.Update(category, categoryName);

        return await _categoryRepository.UpdateCategory(category) ?
            "Категория изменена!" : "Произошла ошибка!";
    }

    public async Task<string> DeleteCategory(string categoryName)
    {
        var category = await _categoryRepository.GetCategoryByName(categoryName);

        if (category is null) 
            throw new Exception("Категория не найдена");

        return await _categoryRepository.DeleteCategory(category) ?
            "Категория удалена!" : "Произошла ошибка!";
    }
}
