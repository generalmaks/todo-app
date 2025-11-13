using Microsoft.AspNetCore.Mvc;
using TodoAppBackend.Application.DTOs.TaskItem;
using TodoAppBackend.Application.Interfaces;
using TodoAppBackend.Application.Services;
using TodoAppBackend.Domain.Entities;

namespace TodoAppBackend.Controllers;

[ApiController]
[Route("/api/tasks")]
public class TaskController(ITaskService taskService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateTaskDto dto)
    {
        var id = await taskService.CreateTaskAsync(dto);
        return Ok(id);
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<TaskItem>>> ListByUser(string userId)
    {
        var tasks = await taskService.GetTasksByUserAsync(userId);
        return Ok(tasks);
    }

    [HttpPut("{taskId:int}")]
    public async Task<IActionResult> Put(int taskId,[FromBody] UpdateTaskDto dto)
    {
        await taskService.UpdateTaskAsync(taskId, dto);
        return NoContent();
    }

    [HttpDelete("{taskId:int}")]
    public async Task<IActionResult> Delete(int taskId)
    {
        await taskService.DeleteTaskAsync(taskId);
        return NoContent();
    }
}