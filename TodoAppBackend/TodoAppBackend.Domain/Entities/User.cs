namespace TodoAppBackend.Domain.Entities;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public ICollection<TaskItem> Tasks { get; set; }
    public ICollection<Category> Categories { get; set; }
}