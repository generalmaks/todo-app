using Microsoft.EntityFrameworkCore;
using TodoAppBackend.Domain.Entities;
using TodoAppBackend.Domain.Interfaces.Repositories;

namespace TodoAppBackend.Infrastructure.Repositories;

public class CategoryRepository(AppDbContext context) : ICategoryRepository
{
    public async Task<Category?> GetByIdAsync(int id)
    {
        return await context.Categories
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task CreateAsync(Category category)
    {
        await context.Categories.AddAsync(category);
        await context.SaveChangesAsync();
    }

    public void Update(Category category)
    {
        context.Update(category);
    }

    public async Task SaveChangesAsync()
    {
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var found = await context.Categories.FindAsync(id);
        if (found is null)
            throw new KeyNotFoundException("Category not found.");
        context.Categories.Remove(found);
        await context.SaveChangesAsync();
    }
}