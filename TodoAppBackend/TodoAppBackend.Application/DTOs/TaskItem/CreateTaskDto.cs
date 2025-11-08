namespace TodoAppBackend.Application.DTOs.TaskItem;

public record CreateTaskDto
{
    public string UserEmail { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsImportant { get; set; }
    public int CategoryId { get; set; }
    public DateTime DueDate { get; set; }
}