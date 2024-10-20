﻿using Microsoft.EntityFrameworkCore;
using Persistance.Data;
using Persistance.Models;
using Persistance.Repository.Interfaces;

namespace Persistance.Repository;

public class CategoryRepository(ForumContext context) : ICategoryRepository
{
    private readonly ForumContext _context = context;

    public async Task<bool> CategoryExistByName(string categoryName)
    {
        return await _context.Categories.AnyAsync(c => c.Name == categoryName);
    }

    public async Task<bool> CategoryExistById(Ulid id)
    {
        return await _context.Categories.AnyAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Category>> GetCategories()
    {
        return await _context.Categories.ToArrayAsync();
    }

    public async Task<Category> GetCategoryById(Ulid id)
    {
        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Category> GetCategoryByName(string categoryName)
    {
        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Name == categoryName);
    }

    public async Task<bool> CreateCategory(Category category)
    {
        _context.Add(category);
        return await Save();
    }

    public async Task<bool> DeleteCategory(Category category)
    {
        _context.Remove(category);
        return await Save();
    }

    public async Task<bool> UpdateCategory(Category category)
    {
        _context.Update(category);
        return await Save();
    }

    public async Task<bool> Save() => await _context.SaveChangesAsync() > 0 ? true : false;
}
