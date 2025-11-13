using Moq;
using TodoAppBackend.Application.DTOs.Category;
using TodoAppBackend.Application.Services;
using TodoAppBackend.Domain.Entities;
using TodoAppBackend.Domain.Interfaces.Repositories;

namespace TodoAppBackend.Tests.Services;

[TestFixture]
public class CategoryServiceTests
{
    private Mock<ICategoryRepository> _categoryRepositoryMock;
    private CategoryService _categoryService;

    [SetUp]
    public void SetUp()
    {
        _categoryRepositoryMock = new Mock<ICategoryRepository>();
        _categoryService = new CategoryService(_categoryRepositoryMock.Object);
    }

    [Test]
    public async Task GetCategoryByIdAsync_WhenCategoryExists_ReturnsCategory()
    {
        // Arrange
        var categoryId = 1;
        var expectedCategory = new Category
        {
            Id = categoryId,
            Name = "Work",
            Description = "Work related tasks",
            UserEmailId = "test@example.com"
        };
        _categoryRepositoryMock.Setup(x => x.GetByIdAsync(categoryId))
            .ReturnsAsync(expectedCategory);

        // Act
        var result = await _categoryService.GetCategoryByIdAsync(categoryId);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.Multiple(() =>
        {
            Assert.That(result.Id, Is.EqualTo(categoryId));
            Assert.That(result.Name, Is.EqualTo("Work"));
            Assert.That(result.Description, Is.EqualTo("Work related tasks"));
            Assert.That(result.UserEmailId, Is.EqualTo("test@example.com"));
        });
        _categoryRepositoryMock.Verify(x => x.GetByIdAsync(categoryId), Times.Once);
    }

    [Test]
    public async Task GetCategoryByIdAsync_WhenCategoryDoesNotExist_ReturnsNull()
    {
        // Arrange
        var categoryId = 999;
        _categoryRepositoryMock.Setup(x => x.GetByIdAsync(categoryId))
            .ReturnsAsync((Category)null!);

        // Act
        var result = await _categoryService.GetCategoryByIdAsync(categoryId);

        // Assert
        Assert.That(result, Is.Null);
        _categoryRepositoryMock.Verify(x => x.GetByIdAsync(categoryId), Times.Once);
    }

    [Test]
    public async Task CreateCategoryAsync_WithValidDto_CreatesCategory()
    {
        // Arrange
        var dto = new CreateCategoryDto
        {
            Name = "Personal",
            Description = "Personal tasks",
            UserEmailId = "test@example.com"
        };

        _categoryRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Category>()))
            .Returns(Task.CompletedTask);

        // Act
        await _categoryService.CreateCategoryAsync(dto);

        // Assert
        _categoryRepositoryMock.Verify(x => x.CreateAsync(It.Is<Category>(c =>
            c.Name == dto.Name &&
            c.Description == dto.Description &&
            c.UserEmailId == dto.UserEmailId
        )), Times.Once);
    }

    [Test]
    public async Task UpdateCategoryAsync_WhenCategoryExists_UpdatesCategory()
    {
        // Arrange
        var categoryId = 1;
        var existingCategory = new Category
        {
            Id = categoryId,
            Name = "OldName",
            Description = "OldDescription",
            UserEmailId = "test@example.com"
        };
        var dto = new UpdateCategoryDto
        {
            Name = "NewName",
            Description = "NewDescription"
        };

        _categoryRepositoryMock.Setup(x => x.GetByIdAsync(categoryId))
            .ReturnsAsync(existingCategory);
        _categoryRepositoryMock.Setup(x => x.Update(It.IsAny<Category>()));
        _categoryRepositoryMock.Setup(x => x.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        await _categoryService.UpdateCategoryAsync(categoryId, dto);

        // Assert
        Assert.That(existingCategory.Name, Is.EqualTo("NewName"));
        _categoryRepositoryMock.Verify(x => x.Update(existingCategory), Times.Once);
        _categoryRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void UpdateCategoryAsync_WhenCategoryDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var categoryId = 999;
        var dto = new UpdateCategoryDto { Name = "NewName" };

        _categoryRepositoryMock.Setup(x => x.GetByIdAsync(categoryId))
            .ReturnsAsync((Category)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _categoryService.UpdateCategoryAsync(categoryId, dto));
        Assert.That(ex.Message, Is.EqualTo("Category not found."));
    }

    [Test]
    public async Task DeleteCategoryAsync_WhenCategoryExists_DeletesCategory()
    {
        // Arrange
        var categoryId = 1;
        var existingCategory = new Category
        {
            Id = categoryId,
            Name = "ToDelete",
            UserEmailId = "test@example.com"
        };

        _categoryRepositoryMock.Setup(x => x.GetByIdAsync(categoryId))
            .ReturnsAsync(existingCategory);
        _categoryRepositoryMock.Setup(x => x.DeleteAsync(categoryId))
            .Returns(Task.CompletedTask);
        _categoryRepositoryMock.Setup(x => x.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        await _categoryService.DeleteCategoryAsync(categoryId);

        // Assert
        _categoryRepositoryMock.Verify(x => x.DeleteAsync(categoryId), Times.Once);
        _categoryRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
    }

    [Test]
    public void DeleteCategoryAsync_WhenCategoryDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var categoryId = 999;
        _categoryRepositoryMock.Setup(x => x.GetByIdAsync(categoryId))
            .ReturnsAsync((Category)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _categoryService.DeleteCategoryAsync(categoryId));
        Assert.That(ex.Message, Is.EqualTo("Category not found."));
    }
}