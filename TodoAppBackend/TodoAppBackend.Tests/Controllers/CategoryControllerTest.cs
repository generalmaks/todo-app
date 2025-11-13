using System.Threading.Tasks;
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
        private Mock<ICategoryService> _mockService = null!;
        private CategoryController _controller = null!;

        [SetUp]
        public void Setup()
        {
            _mockService = new Mock<ICategoryService>();
            _controller = new CategoryController(_mockService.Object);
        }

        [Test]
        public async Task GetCategory_ReturnsOk_WhenCategoryExists()
        {
            // Arrange
            var categoryDto = new Category()
            {
                Id = 1,
                Name = "TestName",
                Description = "Description",
                UserEmailId = "maksym@gmail.com",
            };
            _mockService.Setup(s => s.GetCategoryByIdAsync(1))
                        .ReturnsAsync(categoryDto);

            // Act
            var result = await _controller.GetCategory(1);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(okResult, Is.Not.Null);
                Assert.That(categoryDto, Is.EqualTo(okResult!.Value));
            });
        }

        [Test]
        public async Task GetCategory_ReturnsOk_WithNull_WhenCategoryDoesNotExist()
        {
            // Arrange
            _mockService.Setup(s => s.GetCategoryByIdAsync(It.IsAny<int>()))
                        .ReturnsAsync((Category?)null);

            // Act
            var result = await _controller.GetCategory(999);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.Multiple(() =>
            {
                Assert.That(okResult, Is.Not.Null);
                Assert.That(okResult!.Value, Is.Null);
            });
        }

        [Test]
        public async Task CreateCategory_ReturnsCreated()
        {
            // Arrange
            var createDto = new CreateCategoryDto()
            {
                Name = "Name",
                Description = "Description",
                UserEmailId = "Email@gmail.com"
            };
            _mockService.Setup(s => s.CreateCategoryAsync(createDto))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.CreateCategory(createDto);

            // Assert
            Assert.That(result, Is.InstanceOf<CreatedResult>());
        }

        [Test]
        public async Task UpdateCategory_ReturnsNoContent_WhenModelIsValid()
        {
            // Arrange
            var updateDto = new UpdateCategoryDto
            {
                Name = "Name",
                Description = "Description"
            };
            _mockService.Setup(s => s.UpdateCategoryAsync(1, updateDto))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateCategory(1, updateDto);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }

        [Test]
        public async Task DeleteCategory_ReturnsNoContent()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteCategoryAsync(1))
                        .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteCategory(1);

            // Assert
            Assert.That(result, Is.InstanceOf<NoContentResult>());
        }
    }
}
