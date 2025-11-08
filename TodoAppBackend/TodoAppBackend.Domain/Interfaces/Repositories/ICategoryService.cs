using TodoAppBackend.Domain.Entities;

namespace TodoAppBackend.Domain.Interfaces.Repositories;

public interface ICategoryService
{
    Task<Category> GetByIdAsync(int id, int userId);
    
    Task<IEnumerable<Category>> GetCategoriesAsync(int userId);

    Task<int> GetTotalCategoriesCountAsync(int userId);
    
    Task AddAsync(Category category);
    void Update(Category category);
    void Delete(Category category);
    Task SaveChangesAsync();
}