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
    [HttpGet("{email}")]
    public async Task<ActionResult<User>> GetUserByEmail(string email)
    {
        var users = await userService.GetUserByEmailAsync(email);
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
}