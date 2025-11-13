using Microsoft.EntityFrameworkCore;
using TodoAppBackend.Domain.Entities;

namespace TodoAppBackend.Infrastructure;

public class AppDbContext : DbContext
{
    public DbSet<TaskItem> TaskItems => Set<TaskItem>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Category> Categories => Set<Category>();

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>()
            .HasKey(u => u.Email);

        modelBuilder.Entity<Category>()
            .HasKey(c => c.Id);

        modelBuilder.Entity<TaskItem>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Categories)
            .WithOne()
            .HasForeignKey(t => t.UserEmailId)
            .HasPrincipalKey(u => u.Email)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Tasks)
            .WithOne()
            .HasForeignKey(t => t.UserEmailId)
            .HasPrincipalKey(u => u.Email)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Category>()
            .HasOne(c => c.User)
            .WithMany(u => u.Categories)
            .HasForeignKey(c => c.UserEmailId)
            .HasPrincipalKey(u => u.Email)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<TaskItem>()
            .HasOne(t => t.User)
            .WithMany(u => u.Tasks)
            .HasForeignKey(t => t.UserEmailId)
            .HasPrincipalKey(u => u.Email)
            .OnDelete(DeleteBehavior.Cascade);
    }
}