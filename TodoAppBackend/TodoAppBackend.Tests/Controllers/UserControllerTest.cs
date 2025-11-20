using System.Security.Claims;
using Microsoft.AspNetCore.Http;
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

        private const string AuthEmail = "user@mail.com";

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<IUserService>();
            _controller = new UserController(_mockService.Object);
            
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, "auth@example.com"),
                new Claim(ClaimTypes.Role, "User")
            };
            var identity = new ClaimsIdentity(claims, "mock");
            var user = new ClaimsPrincipal(identity);

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = user
                }
            };

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
        public async Task Put_UserIsOwner_ReturnsNoContent()
        {
            var email = "auth@example.com";

            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(
                new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, "User")
                }, "mock")
            );

            _mockService.Setup(s => s.DeleteUserAsync(email))
                .Returns(Task.CompletedTask);

            var result = await _controller.Delete(email);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }
        
        [Test]
        public async Task Delete_ReturnsNoContent_WhenUserIsAdmin()
        {
            var email = "admin@example.com";
            var targetEmail = "someone@example.com";

            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(
                new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, "Admin")
                }, "mock")
            );

            _mockService.Setup(s => s.DeleteUserAsync(targetEmail))
                .Returns(Task.CompletedTask);

            var result = await _controller.Delete(targetEmail);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }


        [Test]
        public async Task Delete_UserIsOwner_ReturnsNoContent()
        {
            // Arrange
            var email = "auth@example.com";

            _controller.ControllerContext.HttpContext.User = new ClaimsPrincipal(
                new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim(ClaimTypes.Role, "User") // not admin
                }, "mock")
            );
            
            _mockService.Setup(s => s.DeleteUserAsync(email))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(email);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }
    }
}
