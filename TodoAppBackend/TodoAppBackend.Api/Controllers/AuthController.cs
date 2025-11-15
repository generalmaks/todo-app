using Microsoft.AspNetCore.Mvc;
using TodoAppBackend.Application.DTOs.Auth;
using TodoAppBackend.Application.Interfaces;
using TodoAppBackend.Application.Services;

namespace TodoAppBackend.Controllers;

[ApiController]
[Route("/api/login")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginDto dto)
    {
        var result = await authService.LogicAsync(dto);
        return Ok(result);
    }
}