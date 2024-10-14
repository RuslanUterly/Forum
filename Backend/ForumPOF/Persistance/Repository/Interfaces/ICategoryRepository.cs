using Persistance.Models;

namespace Persistance.Repository.Interfaces;

public interface ICategoryRepository
{
    Task<bool> CategoryExistByName(string categoryName);
    Task<bool> CategoryExistById(Ulid id);
    Task<IEnumerable<Category>> GetCategories();
    Task<Category> GetCategoryByName(string categoryName);
    Task<Category> GetCategoryById(Ulid id);
    
    Task<bool> CreateCategory(Category category);
    Task<bool> UpdateCategory(Category category);
    Task<bool> DeleteCategory(Category category);

    Task<bool> Save();
}
