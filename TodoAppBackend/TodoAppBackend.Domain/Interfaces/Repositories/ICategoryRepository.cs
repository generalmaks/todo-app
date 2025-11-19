using TodoAppBackend.Domain.Entities;

namespace TodoAppBackend.Domain.Interfaces.Repositories;

public interface ICategoryRepository
{
    Task<Category?> GetByIdAsync(int id);
    Task<IEnumerable<Category>> GetByUserEmailIdAsync(string userEmailId);
    Task CreateAsync(Category dto);
    void Update(Category category);
    Task SaveChangesAsync();
    Task DeleteAsync(int id);
}