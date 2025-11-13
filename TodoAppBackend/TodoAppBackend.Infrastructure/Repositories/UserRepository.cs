using Microsoft.EntityFrameworkCore;
using TodoAppBackend.Domain.Entities;
using TodoAppBackend.Domain.Interfaces.Repositories;

namespace TodoAppBackend.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context) => _context = context;

    public async Task<User?> GetByEmailAsync(string email) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await SaveChangesAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Update(User user)
    {
        _context.Update(user);
    }

    public async Task RemoveAsync(string emailId)
    {
        var found = await _context.Users.FindAsync(emailId);
        if (found is null)
            throw new KeyNotFoundException("User not found.");
        _context.Remove(found);
        await _context.SaveChangesAsync();
    }
}