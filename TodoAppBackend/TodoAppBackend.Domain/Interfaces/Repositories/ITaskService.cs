using TodoAppBackend.Domain.Entities;

namespace TodoAppBackend.Domain.Interfaces.Repositories;

public interface ITaskService
{
    Task<TaskItem> GetByIdAsync(int id, int userId);

    Task<IEnumerable<TaskItem>> GetTasksAsync(
        int userId,
        int pageNumber,
        int pageSize,
        string? searchTerm,
        int? categoryId
    );

    Task<int> GetTotalTasksCountAsync(int userId, string? searchItem, int? categoryId);
    
    Task AddAsync(TaskItem task);
    void Update(TaskItem task);
    void Delete(TaskItem task);
    Task SaveChangesAsync();
}