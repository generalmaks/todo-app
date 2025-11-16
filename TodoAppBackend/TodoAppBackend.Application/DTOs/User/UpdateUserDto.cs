namespace TodoAppBackend.Application.DTOs.User;

public record UpdateUserDto()
{
    public string OldUnhashedPassword { get; set; }
    public string NewUnhashedPassword { get; set; }
}