using TodoAppBackend.Domain.Entities;

namespace TodoAppBackend.Domain.Interfaces.Repositories;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}