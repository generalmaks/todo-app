using TodoAppBackend.Domain.Entities;

namespace TodoAppBackend.Domain.Interfaces.Repositories;

public interface IUserService
{
    Task<User> GetByEmailAsync(string email);
    Task AddAsync(User user);
    Task SaveChangesAsync();
}