namespace TodoAppBackend.Application.DTOs.Auth;

public record LoginDto(string email, string unhashedPassword);