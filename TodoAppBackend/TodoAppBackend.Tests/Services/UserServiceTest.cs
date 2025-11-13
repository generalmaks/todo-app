using Moq;
using TodoAppBackend.Application.DTOs.User;
using TodoAppBackend.Application.Services;
using TodoAppBackend.Domain.Entities;
using TodoAppBackend.Domain.Interfaces.Repositories;

namespace TodoAppBackend.Tests.Services;

[TestFixture]
public class UserServiceTests
{
    private Mock<IUserRepository> _userRepositoryMock;
    private Mock<IPasswordHasher> _passwordHasherMock;
    private UserService _userService;

    [SetUp]
    public void SetUp()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _passwordHasherMock = new Mock<IPasswordHasher>();
        _userService = new UserService(
            _userRepositoryMock.Object,
            _passwordHasherMock.Object);
    }

    [Test]
    public async Task CreateUserAsync_WithValidData_CreatesUser()
    {
        // Arrange
        var dto = new CreateUserDto
        {
            Email = "newuser@example.com",
            UnhashedPassword = "SecurePassword123"
        };
        var hashedPassword = "hashedPassword123";

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(dto.Email))
            .ReturnsAsync((User)null);
        _passwordHasherMock.Setup(x => x.Hash(dto.UnhashedPassword))
            .Returns(hashedPassword);
        _userRepositoryMock.Setup(x => x.AddAsync(It.IsAny<User>()))
            .Returns(Task.CompletedTask);
        _userRepositoryMock.Setup(x => x.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        await _userService.CreateUserAsync(dto);

        // Assert
        _userRepositoryMock.Verify(x => x.AddAsync(It.Is<User>(u =>
            u.Email == dto.Email &&
            u.PasswordHash == hashedPassword
        )), Times.Once);
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        _passwordHasherMock.Verify(x => x.Hash(dto.UnhashedPassword), Times.Once);
    }

    [Test]
    public void CreateUserAsync_WhenUserAlreadyExists_ThrowsException()
    {
        // Arrange
        var dto = new CreateUserDto
        {
            Email = "existing@example.com",
            UnhashedPassword = "Password123"
        };
        var existingUser = new User { Email = dto.Email };

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(dto.Email))
            .ReturnsAsync(existingUser);

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(
            async () => await _userService.CreateUserAsync(dto));
        Assert.That(ex.Message, Is.EqualTo("User with this email already exists."));
    }

    [Test]
    public void CreateUserAsync_WhenPasswordTooShort_ThrowsException()
    {
        // Arrange
        var dto = new CreateUserDto
        {
            Email = "test@example.com",
            UnhashedPassword = "short"
        };

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(dto.Email))
            .ReturnsAsync((User)null!);

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(
            async () => await _userService.CreateUserAsync(dto));
        Assert.That(ex.Message, Is.EqualTo("Password must be at least 8 character long."));
    }

    [TestCase("")]
    [TestCase("plainaddress")]
    [TestCase("@no-local-part.com")]
    [TestCase("no-at.domain.com")]
    [TestCase("user@.com")]
    [TestCase("user@domain,com")]
    [TestCase("user@domain@domain.com")]
    [TestCase("space in@domain.com")]
    public void CreateUserAsync_WhenInvalidEmail_ThrowsException(string email)
    {
        // Arrange
        var dto = new CreateUserDto
        {
            Email = email,
            UnhashedPassword = "11111111"
        };
        
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(dto.Email))
            .ReturnsAsync((User)null!);

        Assert.ThrowsAsync<ArgumentException>((() => _userService.CreateUserAsync(dto)));
    }

    [Test]
    public async Task GetUserByEmailAsync_WhenUserExists_ReturnsUser()
    {
        // Arrange
        var email = "test@example.com";
        var expectedUser = new User
        {
            Email = email,
            PasswordHash = "hashedPassword"
        };

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync(expectedUser);

        // Act
        var result = await _userService.GetUserByEmailAsync(email);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Email, Is.EqualTo(email));
        _userRepositoryMock.Verify(x => x.GetByEmailAsync(email), Times.Once);
    }

    [Test]
    public void GetUserByEmailAsync_WhenUserDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var email = "notfound@example.com";
        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync((User)null!);

        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _userService.GetUserByEmailAsync(email));
        Assert.That(ex.Message, Is.EqualTo("User not found."));
    }

    [Test]
    public async Task UpdateUserAsync_WithValidData_UpdatesUser()
    {
        // Arrange
        var email = "test@example.com";
        var existingUser = new User
        {
            Email = email,
            PasswordHash = "oldHashedPassword"
        };
        var dto = new UpdateUserDto
        {
            UnhashedPassword = "NewPassword123"
        };
        var newHashedPassword = "newHashedPassword";

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync(existingUser);
        _passwordHasherMock.Setup(x => x.Hash(dto.UnhashedPassword))
            .Returns(newHashedPassword);
        _userRepositoryMock.Setup(x => x.Update(It.IsAny<User>()));
        _userRepositoryMock.Setup(x => x.SaveChangesAsync())
            .Returns(Task.CompletedTask);

        // Act
        await _userService.UpdateUserAsync(email, dto);

        // Assert
        Assert.That(existingUser.PasswordHash, Is.EqualTo(newHashedPassword));
        _userRepositoryMock.Verify(x => x.Update(existingUser), Times.Once);
        _userRepositoryMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        _passwordHasherMock.Verify(x => x.Hash(dto.UnhashedPassword), Times.Once);
    }

    [Test]
    public void UpdateUserAsync_WhenUserDoesNotExist_ThrowsKeyNotFoundException()
    {
        // Arrange
        var email = "notfound@example.com";
        var dto = new UpdateUserDto { UnhashedPassword = "NewPassword123" };

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync((User)null);

        // Act & Assert
        var ex = Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await _userService.UpdateUserAsync(email, dto));
        Assert.That(ex.Message, Is.EqualTo("User not found."));
    }

    [Test]
    public void UpdateUserAsync_WhenPasswordTooShort_ThrowsException()
    {
        // Arrange
        var email = "test@example.com";
        var existingUser = new User { Email = email };
        var dto = new UpdateUserDto { UnhashedPassword = "short" };

        _userRepositoryMock.Setup(x => x.GetByEmailAsync(email))
            .ReturnsAsync(existingUser);

        // Act & Assert
        var ex = Assert.ThrowsAsync<Exception>(
            async () => await _userService.UpdateUserAsync(email, dto));
        Assert.That(ex.Message, Is.EqualTo("Password must be at least 8 character long."));
    }

    [Test]
    public async Task DeleteUserAsync_CallsRepositoryRemoveAsync()
    {
        // Arrange
        var email = "test@example.com";
        _userRepositoryMock.Setup(x => x.RemoveAsync(email))
            .Returns(Task.CompletedTask);

        // Act
        await _userService.DeleteUserAsync(email);

        // Assert
        _userRepositoryMock.Verify(x => x.RemoveAsync(email), Times.Once);
    }
}