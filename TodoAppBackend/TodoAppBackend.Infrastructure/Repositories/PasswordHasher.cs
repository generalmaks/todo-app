using System.Security.Cryptography;
using System.Text;
using TodoAppBackend.Domain.Interfaces.Repositories;

namespace TodoAppBackend.Infrastructure.Repositories;

public class PasswordHasher : IPasswordHasher
{
    public string Hash(string input)
    {
        var inputBytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = SHA256.HashData(inputBytes);
        var hashString = Convert.ToHexString(hashBytes);
        return hashString;
    }
}