using TodoAppBackend.Application.DTOs.User;
using TodoAppBackend.Domain.Entities;

namespace TodoAppBackend.Application.Interfaces;

public interface IUserService
{
    Task CreateUserAsync(CreateUserDto dto);
    Task<User?> GetUserByEmailAsync(string emailId);
    Task UpdateUserAsync(string emailId, UpdateUserDto dto);
    Task DeleteUserAsync(string emailId);
}