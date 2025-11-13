using TodoAppBackend.Application.DTOs.Category;
using TodoAppBackend.Domain.Entities;

namespace TodoAppBackend.Application.Interfaces;

public interface ICategoryService
{
    Task<Category?> GetCategoryByIdAsync(int id);
    Task CreateCategoryAsync(CreateCategoryDto dto);
    Task UpdateCategoryAsync(int categoryId, UpdateCategoryDto dto);
    Task DeleteCategoryAsync(int categoryId);
}