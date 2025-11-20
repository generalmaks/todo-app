using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
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

        private const string AuthEmail = "user@example.com";

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<ITaskService>();
            _controller = new TaskController(_mockService.Object);

            // attach authenticated user
            var context = new DefaultHttpContext();
            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, AuthEmail)
                }, "mock")
            );

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };
        }

        // ---------- Helpers ----------

        private static OkObjectResult GetOk(object? result)
            => result as OkObjectResult ?? throw new AssertionException("Expected OkObjectResult");

        // ---------- Tests ----------

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
                UserEmailId = AuthEmail
            };

            _mockService.Setup(s => s.CreateTaskAsync(dto))
                        .ReturnsAsync(42);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            Assert.That(GetOk(result).Value, Is.EqualTo(42));
        }

        [Test]
        public async Task ListByUser_ReturnsOk_WithTasks()
        {
            // Arrange
            var tasks = new List<TaskItem>
            {
                new TaskItem { Id = 1, Name = "A", UserEmailId = AuthEmail },
                new TaskItem { Id = 2, Name = "B", UserEmailId = AuthEmail }
            };

            _mockService.Setup(s => s.GetTasksByUserAsync(AuthEmail))
                        .ReturnsAsync(tasks);

            // Act
            var result = await _controller.ListByUser(AuthEmail);

            // Assert
            Assert.That(GetOk(result.Result!).Value, Is.EqualTo(tasks));
        }

        [Test]
        public async Task Put_ReturnsNoContent()
        {
            // Arrange
            var user = new User
            {
                Email = AuthEmail,
                PasswordHash = "12341234",
                Role = UserRole.DefaultUser,
                Tasks = null,
                Categories = null
            };

            var task = new TaskItem
            {
                Id = 1,
                Name = "Name",
                Description = null,
                IsCompleted = false,
                IsImportant = false,
                DueDate = default,
                CategoryId = 0,
                UserEmailId = user.Email,
                User = user
            };

            var updateTaskDto = new UpdateTaskDto
            {
                Name = "Updated Task",
                Description = "Updated Desc",
                IsCompleted = true,
                IsImportant = false,
                DueDate = DateTime.UtcNow,
                CategoryId = 3
            };
            
            _mockService.Setup(s => s.GetTaskByIdAsync(1))
                .Returns(Task.FromResult(task));

            _mockService.Setup(s => s.UpdateTaskAsync(1, updateTaskDto))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put(1, updateTaskDto);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task Delete_ReturnsNoContent()
        {
            var user = new User
            {
                Email = AuthEmail,
                PasswordHash = "12341234",
                Role = UserRole.DefaultUser,
                Tasks = null,
                Categories = null
            };

            var task = new TaskItem
            {
                Id = 1,
                Name = "Name",
                Description = null,
                IsCompleted = false,
                IsImportant = false,
                DueDate = default,
                CategoryId = 0,
                UserEmailId = user.Email,
                User = user
            };
            
            // Arrange
            _mockService.Setup(s => s.GetTaskByIdAsync(1))
                .Returns(Task.FromResult(task));
            
            _mockService.Setup(s => s.DeleteTaskAsync(1))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }
    }
}
