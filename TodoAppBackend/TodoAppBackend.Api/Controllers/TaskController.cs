using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateTaskDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        if (userId != dto.UserEmailId)
            return Forbid();
        
        var id = await taskService.CreateTaskAsync(dto);
        return Ok(id);
    }

    [Authorize]
    [HttpGet("{userId}")]
    public async Task<ActionResult<IEnumerable<TaskItem>>> ListByUser(string userId)
    {
        var senderId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(senderId))
            return Unauthorized();

        if (senderId != userId)
            return Forbid();
        
        var tasks = await taskService.GetTasksByUserAsync(userId);
        return Ok(tasks);
    }

    [Authorize]
    [HttpPut("{taskId:int}")]
    public async Task<IActionResult> Put(int taskId,[FromBody] UpdateTaskDto dto)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var foundTask = await taskService.GetTaskByIdAsync(taskId);

        if (foundTask.UserEmailId != userId)
            return Forbid();
        
        await taskService.UpdateTaskAsync(taskId, dto);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{taskId:int}")]
    public async Task<IActionResult> Delete(int taskId)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(userId))
            return Unauthorized();

        var foundTask = await taskService.GetTaskByIdAsync(taskId);

        if (foundTask.UserEmailId != userId)
            return Forbid();
        
        await taskService.DeleteTaskAsync(taskId);
        return NoContent();
    }
}