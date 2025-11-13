namespace TodoAppBackend.Application.DTOs.User;

public record UpdateUserDto()
{
    public string UnhashedPassword { get; set; }
}