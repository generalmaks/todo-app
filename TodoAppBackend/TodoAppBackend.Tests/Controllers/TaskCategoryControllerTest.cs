using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoAppBackend.Controllers;
using TodoAppBackend.Application.DTOs.TaskItem;
using TodoAppBackend.Application.Interfaces;
using TodoAppBackend.Domain.Entities;

namespace TodoAppBackend.Tests.Controllers
{
    [TestFixture]
    public class TaskControllerTests
    {
        private Mock<ITaskService> _mockService = null!;
        private TaskController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<ITaskService>();
            _controller = new TaskController(_mockService.Object);
        }

        [Test]
        public async Task Create_ReturnsOk_WithTaskId()
        {
            // Arrange
            var dto = new CreateTaskDto
            {
                Name = "Test Task",
                Description = "Test Description",
                DueDate = DateTime.UtcNow,
                IsImportant = true,
                CategoryId = 1,
                UserEmailId = "user@example.com"
            };
            _mockService.Setup(s => s.CreateTaskAsync(dto))
                        .ReturnsAsync(42);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(okResult, Is.Not.Null);
                Assert.That(okResult!.Value, Is.EqualTo(42));
            });
        }

        [Test]
        public async Task ListByUser_ReturnsOk_WithTasks()
        {
            // Arrange
            var user = new User { Email = "user@example.com" };
            var tasks = new List<TaskItem>
            {
                new TaskItem
                {
                    Id = 1,
                    Name = "Task1",
                    Description = "Desc1",
                    IsCompleted = false,
                    IsImportant = true,
                    DueDate = DateTime.UtcNow.AddDays(1),
                    CategoryId = 1,
                    UserEmailId = "user@example.com",
                    User = user
                },
                new TaskItem
                {
                    Id = 2,
                    Name = "Task2",
                    Description = "Desc2",
                    IsCompleted = true,
                    IsImportant = false,
                    DueDate = DateTime.UtcNow.AddDays(2),
                    CategoryId = 1,
                    UserEmailId = "user@example.com",
                    User = user
                }
            };
            _mockService.Setup(s => s.GetTasksByUserAsync("user@example.com"))
                        .ReturnsAsync(tasks);

            // Act
            var result = await _controller.ListByUser("user@example.com");

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(okResult, Is.Not.Null);
                Assert.That(okResult!.Value, Is.EqualTo(tasks));
            });
        }

        [Test]
        public async Task Put_ReturnsNoContent()
        {
            // Arrange
            var dto = new UpdateTaskDto
            {
                Name = "Updated Task",
                Description = "Updated Desc",
                IsCompleted = true,
                IsImportant = false,
                DueDate = DateTime.UtcNow.AddDays(3),
                CategoryId = 2
            };
            _mockService.Setup(s => s.UpdateTaskAsync(1, dto))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put(1, dto);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task Delete_ReturnsNoContent()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteTaskAsync(1))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }
    }
}
