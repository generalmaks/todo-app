using TodoAppBackend.Application.DTOs.TaskItem;
using TodoAppBackend.Domain.Entities;

namespace TodoAppBackend.Application.Interfaces;

public interface ITaskService
{
    Task<IEnumerable<TaskItem>> GetTasksByUserAsync(string userEmail);
    Task<int> CreateTaskAsync(CreateTaskDto dto);
    Task UpdateTaskAsync(int taskId, UpdateTaskDto dto);
    Task DeleteTaskAsync(int taskId);
}