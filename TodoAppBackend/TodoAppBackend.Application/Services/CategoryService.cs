using TodoAppBackend.Application.DTOs.Category;
using TodoAppBackend.Application.Interfaces;
using TodoAppBackend.Domain.Entities;
using TodoAppBackend.Domain.Interfaces.Repositories;

namespace TodoAppBackend.Application.Services;

public class CategoryService(
    ICategoryRepository categoryRepository
) : ICategoryService
{
    public async Task<Category?> GetCategoryByIdAsync(int id)
    {
        return await categoryRepository.GetByIdAsync(id);
    }

    public async Task CreateCategoryAsync(CreateCategoryDto dto)
    {
        var category = new Category()
        {
            Name = dto.Name,
            Description = dto.Description,
            UserEmailId = dto.UserEmailId
        };
        await categoryRepository.CreateAsync(category);
    }

    public async Task UpdateCategoryAsync(int categoryId, UpdateCategoryDto dto)
    {
        var found = await categoryRepository.GetByIdAsync(categoryId);
        if (found is null)
            throw new KeyNotFoundException("Category not found.");
        
        found.Name = dto.Name ?? found.Name;
        found.Description = dto.Description ?? dto.Description;
        
        categoryRepository.Update(found);
        await categoryRepository.SaveChangesAsync();
    }

    public async Task DeleteCategoryAsync(int categoryId)
    {
        var found = await categoryRepository.GetByIdAsync(categoryId);
        if (found is null)
            throw new KeyNotFoundException("Category not found.");

        await categoryRepository.DeleteAsync(categoryId);
        await categoryRepository.SaveChangesAsync();
    }
}