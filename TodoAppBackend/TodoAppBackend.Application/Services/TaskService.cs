using TodoAppBackend.Application.DTOs.TaskItem;
using TodoAppBackend.Application.Interfaces;
using TodoAppBackend.Domain.Entities;
using TodoAppBackend.Domain.Interfaces.Repositories;

namespace TodoAppBackend.Application.Services;

public class TaskService(
    ITaskRepository taskRepository,
    IUserRepository userRepository,
    ICategoryRepository categoryRepository) : ITaskService
{
    public async Task<int> CreateTaskAsync(CreateTaskDto dto)
    {
        var user = await userRepository.GetByEmailAsync(dto.UserEmail);
        var category = await categoryRepository.GetByIdAsync(dto.CategoryId);

        if (user is null)
            throw new InvalidOperationException("User not found.");
        if (category is null)
            throw new InvalidOperationException("Category not found.");

        var taskItem = new TaskItem
        {
            Name = dto.Name,
            Description = dto.Description,
            IsImportant = dto.IsImportant,
            DueDate = dto.DueDate,
            CategoryId = dto.CategoryId
        };
        await taskRepository.AddAsync(taskItem);
        await taskRepository.SaveChangesAsync();

        return taskItem.Id;
    }

    public async Task UpdateTaskAsync(int taskId, UpdateTaskDto dto)
    {
        var found = await taskRepository.GetByIdAsync(taskId);
        if (found is null)
            throw new KeyNotFoundException("Task not found.");
        
        found.Name = dto.Name ?? found.Name;
        found.Description = dto.Description ?? found.Description;
        found.IsImportant = dto.IsImportant ?? found.IsImportant;
        found.CategoryId = dto.CategoryId ?? found.CategoryId;
        
        taskRepository.Update(found);
        await taskRepository.SaveChangesAsync();
    }

    public async Task DeleteTaskAsync(int taskId)
    {
        var found = await taskRepository.GetByIdAsync(taskId);
        if (found is null)
            throw new KeyNotFoundException("Task not found.");
        taskRepository.Delete(found);
    }

    public async Task<IEnumerable<TaskItem>> GetTasksByUserAsync(string userEmail)
    {
        // TODO: Refactor magic numbers
        const int pageNumber = 1;
        const int pageSize = 10;
        return await taskRepository.GetTasksAsync(
            userEmail, 
            pageNumber, 
            pageSize,
            null,
            null);
    }

    public async Task MarkAsDoneAsync(int taskId)
    {
        var taskItem = await taskRepository.GetByIdAsync(taskId);
        if (taskItem is null)
            throw new InvalidOperationException("Task not found.");

        taskItem.IsCompleted = true;
        await taskRepository.SaveChangesAsync();
    }
}