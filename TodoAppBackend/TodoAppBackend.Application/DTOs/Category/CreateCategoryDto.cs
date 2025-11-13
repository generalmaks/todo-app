namespace TodoAppBackend.Application.DTOs.Category;

public record CreateCategoryDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string UserEmailId { get; set; }
}