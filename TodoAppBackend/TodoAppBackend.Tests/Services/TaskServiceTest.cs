using Moq;
using TodoAppBackend.Application.DTOs.TaskItem;
using TodoAppBackend.Application.Services;
using TodoAppBackend.Domain.Entities;
using TodoAppBackend.Domain.Interfaces.Repositories;

namespace TodoAppBackend.Tests.Services;

[TestFixture]
public class TaskServiceTests
{
    private Mock<ITaskRepository> _taskRepositoryMock;
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private TaskService _taskService;

    [SetUp]
    public void SetUp()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _taskService = new TaskService(
            _taskRepositoryMock.Object,
            _userRepositoryMock.Object,
            _categoryRepositoryMock.Object);
    }

    [Test]
    public async Task CreateTaskAsync_WithValidData_CreatesTaskAndReturnsId()
    {
        // Arrange
        var dto = new CreateTaskDto
        {
            Name = "New Task",
            Description = "Task description",
            IsImportant = true,
            DueDate = DateTime.Now.AddDays(7),
            CategoryId = 1,
            UserEmailId = "test@example.com"
        };

        var user = new User { Email = dto.UserEmailId };
        var category = new Category { Id = dto.CategoryId };

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(dto.UserEmailId))
            .ReturnsAsync(user);
        _categoryRepositoryMock.Setup(x => x.GetByIdAsync(dto.CategoryId))
            .ReturnsAsync(category);
        _taskRepositoryMock.Setup(x => x.AddAsync(It.IsAny<TaskItem>()))
            .Callback<TaskItem>(t => t.Id = 123)
            .Returns(Task.CompletedTask);
        _taskRepositoryMock.Setup(x => x.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        var result = await _taskService.CreateTaskAsync(dto);

        // Assert
        Assert.That(result, Is.EqualTo(123));
        _taskRepositoryMock.Verify(x => x.AddAsync(It.Is<TaskItem>(t =>
            t.Name == dto.Name &&
            t.Description == dto.Description &&
            t.IsImportant == dto.IsImportant &&
            t.CategoryId == dto.CategoryId &&
            t.UserEmailId == dto.UserEmailId
        )), Times.Once);
        _taskRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void CreateTaskAsync_WhenUserNotFound_ThrowsInvalidOperationException()
    {
        // Arrange
        var dto = new CreateTaskDto
        {
            Name = "Task",
            CategoryId = 1,
            UserEmailId = "notfound@example.com"
        };

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(dto.UserEmailId))
            .ReturnsAsync((User)null);
        _categoryRepositoryMock.Setup(x => x.GetByIdAsync(dto.CategoryId))
            .ReturnsAsync(new Category());

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _taskService.CreateTaskAsync(dto));
        Assert.That(ex.Message, Is.EqualTo("User not found."));
    }

    [Test]
    public void CreateTaskAsync_WhenCategoryNotFound_ThrowsInvalidOperationException()
    {
        // Arrange
        var dto = new CreateTaskDto
        {
            Name = "Task",
            CategoryId = 999,
            UserEmailId = "test@example.com"
        };

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(dto.UserEmailId))
            .ReturnsAsync(new User());
        _categoryRepositoryMock.Setup(x => x.GetByIdAsync(dto.CategoryId))
            .ReturnsAsync((Category)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _taskService.CreateTaskAsync(dto));
        Assert.That(ex.Message, Is.EqualTo("Category not found."));
    }

    [Test]
    public async Task UpdateTaskAsync_WhenTaskExists_UpdatesTask()
    {
        // Arrange
        var taskId = 1;
        var existingTask = new TaskItem
        {
            Id = taskId,
            Name = "OldName",
            Description = "OldDescription",
            IsImportant = false,
            CategoryId = 1
        };
        var dto = new UpdateTaskDto
        {
            Name = "NewName",
            Description = "NewDescription",
            IsImportant = true,
            CategoryId = 2
        };

        _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync(existingTask);
        _taskRepositoryMock.Setup(x => x.Update(It.IsAny<TaskItem>()));
        _taskRepositoryMock.Setup(x => x.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        await _taskService.UpdateTaskAsync(taskId, dto);

        // Assert
        Assert.That(existingTask.Name, Is.EqualTo("NewName"));
        Assert.That(existingTask.Description, Is.EqualTo("NewDescription"));
        Assert.That(existingTask.IsImportant, Is.True);
        Assert.That(existingTask.CategoryId, Is.EqualTo(2));
        _taskRepositoryMock.Verify(x => x.Update(existingTask), Times.Once);
        _taskRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void UpdateTaskAsync_WhenTaskDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var taskId = 999;
        var dto = new UpdateTaskDto { Name = "NewName" };

        _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync((TaskItem)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _taskService.UpdateTaskAsync(taskId, dto));
        Assert.That(ex.Message, Is.EqualTo("Task not found."));
    }

    [Test]
    public async Task DeleteTaskAsync_WhenTaskExists_DeletesTask()
    {
        // Arrange
        var taskId = 1;
        var existingTask = new TaskItem { Id = taskId, Name = "ToDelete" };

        _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync(existingTask);
        _taskRepositoryMock.Setup(x => x.Delete(It.IsAny<TaskItem>()));
        _taskRepositoryMock.Setup(x => x.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        await _taskService.DeleteTaskAsync(taskId);

        // Assert
        _taskRepositoryMock.Verify(x => x.Delete(existingTask), Times.Once);
        _taskRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void DeleteTaskAsync_WhenTaskDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var taskId = 999;
        _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync((TaskItem)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _taskService.DeleteTaskAsync(taskId));
        Assert.That(ex.Message, Is.EqualTo("Task not found."));
    }

    [Test]
    public async Task GetTasksByUserAsync_ReturnsUserTasks()
    {
        // Arrange
        var userEmail = "test@example.com";
        var expectedTasks = new List<TaskItem>
        {
            new TaskItem { Id = 1, Name = "Task 1", UserEmailId = userEmail },
            new TaskItem { Id = 2, Name = "Task 2", UserEmailId = userEmail }
        };

        _taskRepositoryMock.Setup(x => x.GetTasksAsync(
            userEmail, 1, 10, null, null))
            .ReturnsAsync(expectedTasks);

        // Act
        var result = await _taskService.GetTasksByUserAsync(userEmail);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Count(), Is.EqualTo(2));
        _taskRepositoryMock.Verify(x => x.GetTasksAsync(
            userEmail, 1, 10, null, null), Times.Once);
    }

    [Test]
    public async Task MarkAsDoneAsync_WhenTaskExists_MarksTaskAsCompleted()
    {
        // Arrange
        var taskId = 1;
        var task = new TaskItem
        {
            Id = taskId,
            Name = "Task",
            IsCompleted = false
        };

        _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync(task);
        _taskRepositoryMock.Setup(x => x.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        await _taskService.MarkAsDoneAsync(taskId);

        // Assert
        Assert.That(task.IsCompleted, Is.True);
        _taskRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void MarkAsDoneAsync_WhenTaskDoesNotExist_ThrowsInvalidOperationException()
    {
        // Arrange
        var taskId = 999;
        _taskRepositoryMock.Setup(x => x.GetByIdAsync(taskId))
            .ReturnsAsync((TaskItem)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<InvalidOperationException>(
            async () => await _taskService.MarkAsDoneAsync(taskId));
        Assert.That(ex.Message, Is.EqualTo("Task not found."));
    }
}