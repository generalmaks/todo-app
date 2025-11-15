using TodoAppBackend.Application.DTOs.Auth;

namespace TodoAppBackend.Application.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LogicAsync(LoginDto dto);
}