namespace TodoAppBackend.Application.DTOs.User;

public record CreateUserDto
{
    public string Email { get; set; }
    public string UnhashedPassword { get; set; }
}