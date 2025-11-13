using TodoAppBackend.Application.DTOs.User;
using TodoAppBackend.Application.Interfaces;
using TodoAppBackend.Domain.Entities;
using TodoAppBackend.Domain.Interfaces.Repositories;

namespace TodoAppBackend.Application.Services;

public class UserService(
    IUserRepository userRepository,
    IPasswordHasher hasher
) : IUserService
{
    public async Task CreateUserAsync(CreateUserDto dto)
    {
        if (await userRepository.GetByEmailAsync(dto.Email) is not null)
            throw new Exception("User with this email already exists.");
        var hashedPassword = hasher.Hash(dto.UnhashedPassword);
        var newUser = new User()
        {
            Email = dto.Email,
            PasswordHash = hashedPassword
        };
        await userRepository.AddAsync(newUser);
        await userRepository.SaveChangesAsync();
    }

    public async Task<User?> GetUserByEmailAsync(string emailId)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateUserAsync(string emailId, UpdateUserDto dto)
    {
        var found = await userRepository.GetByEmailAsync(emailId);
        if (found is null)
            throw new KeyNotFoundException("User not found.");
        var hashedPassword = hasher.Hash(dto.UnhashedPassword);
        userRepository.Update(found);
        await userRepository.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(string emailId)
    {
        await userRepository.RemoveAsync(emailId);
    }
}