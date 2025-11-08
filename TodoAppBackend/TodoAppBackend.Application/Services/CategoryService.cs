using TodoAppBackend.Application.DTOs.Category;
using TodoAppBackend.Domain.Entities;
using TodoAppBackend.Domain.Interfaces.Repositories;

namespace TodoAppBackend.Application.Services;

public class CategoryService(
    ITaskRepository taskRepository,
    IUserRepository userRepository,
    ICategoryRepository categoryRepository
)
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
            Description = dto.Description
        };
        await categoryRepository.CreateAsync(category);
    }
}