using Application.Helper;
using AutoMapper;
using Persistance.Dto.Categories;
using Persistance.Models;
using Persistance.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services;

public class CategoriesService(
    ICategoryRepository categoryRepository,
    IMapper mapper)
{
    private readonly ICategoryRepository _categoryRepository = categoryRepository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CategoryDetailsRequest>> RecieveAll()
    {
        var categories = await _categoryRepository.GetCategories();

        return _mapper.Map<IEnumerable<CategoryDetailsRequest>>(categories);
    }

    public async Task<CategoryDetailsRequest> RecieveByName(string categoryName)
    {
        var category = await _categoryRepository.GetCategoryByName(categoryName);

        return _mapper.Map<CategoryDetailsRequest>(category);
    }

    //public async Task<Result> CreateCategory(string categoryName)
    //{
    //    if (await _categoryRepository.CategoryExistByName(categoryName))
    //        return Result.Failure("Категория уже создана");

    //    var category = Category.Create(Ulid.NewUlid(), categoryName, DateTime.Now);
        
    //    var isCreated = await _categoryRepository.CreateCategory(category);

    //    return isCreated ?
    //        Result.Success("Категория создана") :
    //        Result.Failure("Произошла ошибка");
    //}
    
    public async Task<Result> CreateCategory(CategoryCreateRequest categoryRequest)
    {
        if (await _categoryRepository.CategoryExistByName(categoryRequest.Name))
            return Result.Failure("Категория уже создана");

        var category = _mapper.Map<Category>(categoryRequest);
        
        var isCreated = await _categoryRepository.CreateCategory(category);

        return isCreated ?
            Result.Success("Категория создана") :
            Result.Failure("Произошла ошибка");
    }

    //public async Task<Result> UpdateCategory(Ulid ulid, string categoryName)
    //{
    //    if (!await _categoryRepository.CategoryExistById(ulid))
    //        return Result.Failure("Категория не найдена");

    //    var category = await _categoryRepository.GetCategoryById(ulid);

    //    category = Category.Update(category, categoryName);

    //    var isUpdated = await _categoryRepository.UpdateCategory(category);

    //    return isUpdated ?
    //        Result.Success("Категория изменена!") : 
    //        Result.Failure("Произошла ошибка!");
    //}
    
    public async Task<Result> UpdateCategory(CategoryUpdateRequest categoryRequest)
    {
        if (!await _categoryRepository.CategoryExistById(categoryRequest.Id))
            return Result.Failure("Категория не найдена");

        if (await _categoryRepository.CategoryExistByName(categoryRequest.Name))
            return Result.Failure("Категория уже создана");

        var category = await _categoryRepository.GetCategoryById(categoryRequest.Id);

        _mapper.Map(categoryRequest, category);

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
