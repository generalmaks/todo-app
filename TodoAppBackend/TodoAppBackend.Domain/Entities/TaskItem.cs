namespace TodoAppBackend.Domain.Entities;

public class TaskItem
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsImportant { get; set; }
    public DateTime DueDate { get; set; }
    public int CategoryId { get; set; }
    public string UserEmailId { get; set; }
    public User User { get; set; }
}