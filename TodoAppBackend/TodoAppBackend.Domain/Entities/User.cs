namespace TodoAppBackend.Domain.Entities;

public class User
{
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public UserRole Role { get; set; }
    public ICollection<TaskItem> Tasks { get; set; }
    public ICollection<Category> Categories { get; set; }
}