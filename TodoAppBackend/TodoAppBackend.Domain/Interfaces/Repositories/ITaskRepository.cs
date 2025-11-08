using TodoAppBackend.Domain.Entities;

namespace TodoAppBackend.Domain.Interfaces.Repositories;

public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(int id);

    Task<IEnumerable<TaskItem>> GetTasksAsync(
        string userEmail,
        int pageNumber,
        int pageSize,
        string? searchTerm,
        int? categoryId
    );

    Task<int> GetTotalTasksCountAsync(string userEmail, string? searchItem, int? categoryId);
    
    Task AddAsync(TaskItem task);
    void Update(TaskItem task);
    void Delete(TaskItem task);
    Task SaveChangesAsync();
}