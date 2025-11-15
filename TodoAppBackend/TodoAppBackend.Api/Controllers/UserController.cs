using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TodoAppBackend.Application.DTOs.User;
using TodoAppBackend.Application.Interfaces;
using TodoAppBackend.Application.Services;
using TodoAppBackend.Domain.Entities;

namespace TodoAppBackend.Controllers;

[ApiController]
[Route("/api/users")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("{userEmailId}")]
    public async Task<ActionResult<User>> GetUserByEmail(string userEmailId)
    {
        var users = await userService.GetUserByEmailAsync(userEmailId);
        return Ok(users);
    }

    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody] CreateUserDto dto)
    {
        if (string.IsNullOrEmpty(dto.Email) || 
            string.IsNullOrEmpty(dto.UnhashedPassword))
            return Unauthorized("No email or password was provided.");
        await userService.CreateUserAsync(dto);
        return Created();
    }

    [Authorize]
    [HttpPut("{userEmailId}")]
    public async Task<IActionResult> Put(string userEmailId, [FromBody] UpdateUserDto dto)
    {
        var senderEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        
        if (senderEmail is null)
            return Unauthorized("User is not authenticated.");
        
        if (!string.Equals(senderEmail, userEmailId, StringComparison.OrdinalIgnoreCase))
            return Forbid("User is not authorized.");
        
        await userService.UpdateUserAsync(userEmailId, dto);
        return NoContent();
    }

    [Authorize]
    [HttpDelete("{userEmailId}")]
    public async Task<IActionResult> Delete(string userEmailId)
    {
        var senderEmail = User.FindFirst(ClaimTypes.Email)?.Value;
        var senderRole = User.FindFirst(ClaimTypes.Role)?.Value;

        if (string.IsNullOrEmpty(userEmailId))
            return BadRequest("Email is not specified.");

        var isOwner = senderEmail == userEmailId;
        var isAdmin = senderRole == nameof(UserRole.Admin);

        if (!isOwner && !isAdmin)
            return Forbid("User is not authorized.");
        
        await userService.DeleteUserAsync(userEmailId);
        return NoContent();
    }
}