using TodoAppBackend.Application.DTOs.User;
using TodoAppBackend.Application.Interfaces;
using TodoAppBackend.Domain.Entities;
using TodoAppBackend.Domain.Interfaces.Repositories;
using System.Net.Mail;

namespace TodoAppBackend.Application.Services;

public class UserService(
    IUserRepository userRepository,
    IPasswordHasher hasher
) : IUserService
{
    public async Task CreateUserAsync(CreateUserDto dto)
    {
        if (!IsValidEmail(dto.Email))
            throw new ArgumentException("Invalid email provided.");
        
        if (dto.UnhashedPassword.Length < 8)
            throw new Exception("Password must be at least 8 character long.");
        
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
        var found = await userRepository.GetByEmailAsync(emailId);
        return found ?? throw new KeyNotFoundException("User not found.");
    }

    public async Task UpdateUserAsync(string emailId, UpdateUserDto dto)
    {
        var found = await userRepository.GetByEmailAsync(emailId);
        if (found is null)
            throw new KeyNotFoundException("Invalid credentials.");

        var oldHashedPassword = hasher.Hash(dto.OldUnhashedPassword);

        if (oldHashedPassword != found.PasswordHash)
            throw new Exception("Invalid credentials");
        
        if (dto.NewUnhashedPassword.Length < 8)
            throw new Exception("Password must be at least 8 character long.");
        
        var hashedPassword = hasher.Hash(dto.NewUnhashedPassword);
        found.PasswordHash = hashedPassword;
        userRepository.Update(found);
        await userRepository.SaveChangesAsync();
    }

    public async Task DeleteUserAsync(string emailId)
    {
        await userRepository.RemoveAsync(emailId);
    }
    
    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}