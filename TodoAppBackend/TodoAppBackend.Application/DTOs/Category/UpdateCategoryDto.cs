namespace TodoAppBackend.Application.DTOs.Category;

public record UpdateCategoryDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
}