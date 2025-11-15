using TodoAppBackend.Application.DTOs.Auth;
using TodoAppBackend.Domain.Entities;

namespace TodoAppBackend.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}