using Microsoft.AspNetCore.Mvc;
using Moq;
using TodoAppBackend.Controllers;
using TodoAppBackend.Application.DTOs.User;
using TodoAppBackend.Application.Interfaces;
using TodoAppBackend.Domain.Entities;

namespace TodoAppBackend.Tests.Controllers
{
    [TestFixture]
    public class UserControllerTests
    {
        private Mock<IUserService> _mockService = null!;
        private UserController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IUserService>();
            _controller = new UserController(_mockService.Object);
        }

        [Test]
        public async Task GetUserByEmail_ReturnsOk_WithUser()
        {
            // Arrange
            var user = new User
            {
                Email = "test@example.com",
                PasswordHash = "hashedpwd"
            };
            _mockService.Setup(s => s.GetUserByEmailAsync("test@example.com"))
                        .ReturnsAsync(user);

            // Act
            var result = await _controller.GetUserByEmail("test@example.com");

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(okResult, Is.Not.Null);
                Assert.That(okResult!.Value, Is.EqualTo(user));
            });
        }

        [Test]
        public async Task GetUserByEmail_ReturnsOk_WithNull_WhenNotFound()
        {
            // Arrange
            _mockService.Setup(s => s.GetUserByEmailAsync(It.IsAny<string>()))
                        .ReturnsAsync((User?)null);

            // Act
            var result = await _controller.GetUserByEmail("notfound@example.com");

            // Assert
            var okResult = result.Result as OkObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(okResult, Is.Not.Null);
                Assert.That(okResult!.Value, Is.Null);
            });
        }

        [Test]
        public async Task CreateUser_ReturnsCreated_WhenValid()
        {
            // Arrange
            var dto = new CreateUserDto
            {
                Email = "test@example.com",
                UnhashedPassword = "password123"
            };
            _mockService.Setup(s => s.CreateUserAsync(dto))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateUser(dto);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedResult>());
        }

        [Test]
        public async Task CreateUser_ReturnsUnauthorized_WhenEmailOrPasswordMissing()
        {
            // Arrange
            var dto = new CreateUserDto { Email = null!, UnhashedPassword = null! };

            // Act
            var result = await _controller.CreateUser(dto);

            // Assert
            var unauthorizedResult = result as UnauthorizedObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(unauthorizedResult, Is.Not.Null);
                Assert.That(unauthorizedResult!.Value, Is.EqualTo("No email or password was provided."));
            });
        }

        [Test]
        public async Task Put_ReturnsNoContent()
        {
            // Arrange
            var dto = new UpdateUserDto { UnhashedPassword = "newpassword" };
            _mockService.Setup(s => s.UpdateUserAsync("test@example.com", dto))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Put("test@example.com", dto);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task Delete_ReturnsNoContent()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteUserAsync("test@example.com"))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete("test@example.com");

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }
    }
}
