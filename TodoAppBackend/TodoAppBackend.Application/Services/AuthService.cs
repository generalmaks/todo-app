using TodoAppBackend.Application.DTOs.Auth;
using TodoAppBackend.Application.Interfaces;
using TodoAppBackend.Domain.Interfaces.Repositories;

namespace TodoAppBackend.Application.Services;

public class AuthService(
    IUserRepository userRepository,
    IPasswordHasher hasher,
    IJwtTokenGenerator tokenGenerator
) : IAuthService
{
    public async Task<AuthResponseDto> LogicAsync(LoginDto dto)
    {
        var user = await userRepository.GetByEmailAsync(dto.email);
        if (user is null || user.PasswordHash != hasher.Hash(dto.unhashedPassword))
            throw new ArgumentException("Wrong credentials.");

        var token = tokenGenerator.GenerateToken(user);

        return new AuthResponseDto(token, user.Email);
    }
}