using AutoMapper;
using FluentAssertions;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Models;
using JobApplicationTracker.Repository;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Identity;
using Moq;

namespace Test.Services;

public class UserServiceTests
{
    private readonly Mock<IUserRepository> _mockRepository;
    private readonly Mock<UserManager<User>> _mockUserManager;
    private readonly Mock<IMapper> _mockMapper;
    private readonly UserService _service;

    public UserServiceTests()
    {
        _mockRepository = new Mock<IUserRepository>();
        var store = new Mock<IUserStore<User>>();
        _mockUserManager = new Mock<UserManager<User>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
        _mockMapper = new Mock<IMapper>();
        _service = new UserService(_mockRepository.Object, _mockUserManager.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllUsers()
    {
        // Arrange
        var users = new List<User>
        {
            new() { Id = "user1", UserName = "alice", Email = "alice@test.com" },
            new() { Id = "user2", UserName = "bob", Email = "bob@test.com" }
        };
        var userDtos = new List<UserDto>
        {
            new() { Id = "user1", UserName = "alice", Email = "alice@test.com" },
            new() { Id = "user2", UserName = "bob", Email = "bob@test.com" }
        };

        _mockRepository.Setup(r => r.GetAllAsync()).ReturnsAsync(users);
        _mockMapper.Setup(m => m.Map<UserDto>(It.IsAny<User>()))
            .Returns((User u) => userDtos.First(dto => dto.Id == u.Id));

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        result.Should().HaveCount(2);
        _mockRepository.Verify(r => r.GetAllAsync(), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsUser_WhenExists()
    {
        // Arrange
        var user = new User { Id = "user1", UserName = "testuser", Email = "test@test.com" };
        var userDto = new UserDto { Id = "user1", UserName = "testuser", Email = "test@test.com" };

        _mockRepository.Setup(r => r.GetByIdAsync("user1")).ReturnsAsync(user);
        _mockMapper.Setup(m => m.Map<UserDto>(user)).Returns(userDto);

        // Act
        var result = await _service.GetByIdAsync("user1");

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be("user1");
        result.UserName.Should().Be("testuser");
        _mockRepository.Verify(r => r.GetByIdAsync("user1"), Times.Once);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetByIdAsync("nonexistent")).ReturnsAsync((User?)null);

        // Act
        var result = await _service.GetByIdAsync("nonexistent");

        // Assert
        result.Should().BeNull();
        _mockRepository.Verify(r => r.GetByIdAsync("nonexistent"), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsTrue_WhenDeleteSucceeds()
    {
        // Arrange
        _mockRepository.Setup(r => r.DeleteAsync("user1")).ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync("user1");

        // Assert
        result.Should().BeTrue();
        _mockRepository.Verify(r => r.DeleteAsync("user1"), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ReturnsFalse_WhenDeleteFails()
    {
        // Arrange
        _mockRepository.Setup(r => r.DeleteAsync("nonexistent")).ReturnsAsync(false);

        // Act
        var result = await _service.DeleteAsync("nonexistent");

        // Assert
        result.Should().BeFalse();
        _mockRepository.Verify(r => r.DeleteAsync("nonexistent"), Times.Once);
    }
}