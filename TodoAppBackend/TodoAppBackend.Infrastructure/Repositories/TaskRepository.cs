using Microsoft.EntityFrameworkCore;
using TodoAppBackend.Domain.Entities;
using TodoAppBackend.Domain.Interfaces.Repositories;

namespace TodoAppBackend.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;
    public TaskRepository(AppDbContext context) => _context = context;

    public async Task<TaskItem?> GetByIdAsync(int id) =>
        await _context.TaskItems.FindAsync(id);

    public async Task<IEnumerable<TaskItem>> GetTasksAsync(
        string userEmail,
        int pageNumber,
        int pageSize,
        string? searchTerm,
        int? categoryId)
    {
        var query = _context.TaskItems.AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(t => t.CategoryId == categoryId.Value);

        if (!string.IsNullOrWhiteSpace(searchTerm))
            query = query.Where(t => t.Name.Contains(searchTerm));

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetTotalTasksCountAsync(string userEmail, string? searchItem, int? categoryId)
    {
        var query = _context.TaskItems.AsQueryable();

        if (categoryId.HasValue)
            query = query.Where(t => t.CategoryId == categoryId.Value);

        if (!string.IsNullOrWhiteSpace(searchItem))
            query = query.Where(t => t.Name.Contains(searchItem));

        return await query.CountAsync();
    }

    public async Task AddAsync(TaskItem task) => await _context.TaskItems.AddAsync(task);

    public void Update(TaskItem task) => _context.TaskItems.Update(task);

    public void Delete(TaskItem task) => _context.TaskItems.Remove(task);

    public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
}