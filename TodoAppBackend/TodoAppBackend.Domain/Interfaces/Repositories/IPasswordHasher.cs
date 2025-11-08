namespace TodoAppBackend.Domain.Interfaces.Repositories;

public interface IPasswordHasher
{
    string Hash(string input);
}