using TodoAppBackend.Application.DTOs.User;
using TodoAppBackend.Domain.Entities;
using TodoAppBackend.Domain.Interfaces.Repositories;

namespace TodoAppBackend.Application.Services;

public class UserService(
    IUserRepository userRepository,
    IPasswordHasher hasher
)
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

    public async Task<User?> GetUserByEmail(string email)
    {
        return await userRepository.GetByEmailAsync(email);
    }
}