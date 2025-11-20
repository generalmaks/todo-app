using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using TodoAppBackend.Controllers;
using TodoAppBackend.Application.DTOs.Category;
using TodoAppBackend.Application.Interfaces;
using TodoAppBackend.Domain.Entities;

namespace TodoAppBackend.Tests.Controllers
{
    [TestFixture]
    public class CategoryControllerTests
    {
        private const string TestEmail = "maksym@gmail.com";

        private Mock<ICategoryService> _mockService = null!;
        private CategoryController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<ICategoryService>();
            _controller = new CategoryController(_mockService.Object);

            var context = new DefaultHttpContext();
            context.User = new ClaimsPrincipal(
                new ClaimsIdentity(new[] { new Claim(ClaimTypes.NameIdentifier, TestEmail) })
            );

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = context
            };
        }

        [Test]
        public async Task GetCategory_ReturnsOk_WhenCategoryExists_AndOwnedByUser()
        {
            var category = new Category
            {
                Id = 1,
                Name = "Test",
                Description = "Desc",
                UserEmailId = TestEmail
            };

            _mockService.Setup(s => s.GetCategoryByIdAsync(1))
                .ReturnsAsync(category);

            var result = await _controller.GetCategory(1);

            var ok = result as OkObjectResult;
            Assert.That(ok, Is.Not.Null);
            Assert.That(ok!.Value, Is.EqualTo(category));
        }

        [Test]
        public async Task GetCategory_ReturnsForbid_WhenCategoryExists_ButNotOwned()
        {
            _mockService.Setup(s => s.GetCategoryByIdAsync(1))
                .ReturnsAsync(new Category { UserEmailId = "someone_else@gmail.com" });

            var result = await _controller.GetCategory(1);

            Assert.That(result, Is.InstanceOf<ForbidResult>());
        }

        [Test]
        public async Task GetCategory_ReturnsForbid_WhenCategoryDoesNotExist()
        {
            _mockService.Setup(s => s.GetCategoryByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((Category?)null);

            var result = await _controller.GetCategory(123);

            Assert.That(result, Is.InstanceOf<ForbidResult>());
        }

        [Test]
        public async Task CreateCategory_ReturnsCreated_WhenEmailMatchesUser()
        {
            var dto = new CreateCategoryDto
            {
                Name = "Cat",
                Description = "Desc",
                UserEmailId = TestEmail
            };

            _mockService.Setup(s => s.CreateCategoryAsync(dto))
                .Returns(Task.CompletedTask);

            var result = await _controller.CreateCategoryAsync(dto);

            Assert.That(result, Is.InstanceOf<CreatedResult>());
        }

        [Test]
        public async Task CreateCategory_ReturnsForbid_WhenUserEmailDoesNotMatch()
        {
            var dto = new CreateCategoryDto
            {
                Name = "Cat",
                Description = "Desc",
                UserEmailId = "someone_else@gmail.com"
            };

            var result = await _controller.CreateCategoryAsync(dto);

            Assert.That(result, Is.InstanceOf<ForbidResult>());
        }

        [Test]
        public async Task UpdateCategory_ReturnsNoContent_WhenOwnedByUser()
        {
            var dto = new UpdateCategoryDto { Name = "New", Description = "Desc" };

            _mockService.Setup(s => s.GetCategoryByIdAsync(1))
                .ReturnsAsync(new Category { UserEmailId = TestEmail });

            _mockService.Setup(s => s.UpdateCategoryAsync(1, dto))
                .Returns(Task.CompletedTask);

            var result = await _controller.UpdateCategory(1, dto);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task UpdateCategory_ReturnsForbid_WhenNotOwnedByUser()
        {
            var dto = new UpdateCategoryDto { Name = "New", Description = "Desc" };

            _mockService.Setup(s => s.GetCategoryByIdAsync(1))
                .ReturnsAsync(new Category { UserEmailId = "someone_else@gmail.com" });

            var result = await _controller.UpdateCategory(1, dto);

            Assert.That(result, Is.InstanceOf<ForbidResult>());
        }

        [Test]
        public async Task DeleteCategory_ReturnsNoContent_WhenOwnedByUser()
        {
            _mockService.Setup(s => s.GetCategoryByIdAsync(1))
                .ReturnsAsync(new Category { UserEmailId = TestEmail });

            _mockService.Setup(s => s.DeleteCategoryAsync(1))
                .Returns(Task.CompletedTask);

            var result = await _controller.DeleteCategory(1);

            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteCategory_ReturnsForbid_WhenNotOwned()
        {
            _mockService.Setup(s => s.GetCategoryByIdAsync(1))
                .ReturnsAsync(new Category { UserEmailId = "someone_else@gmail.com" });

            var result = await _controller.DeleteCategory(1);

            Assert.That(result, Is.InstanceOf<ForbidResult>());
        }
    }
}
