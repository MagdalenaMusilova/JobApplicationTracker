using FluentAssertions;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Dos;
using JobApplicationTracker.Repository;
using JobApplicationTracker.Services;
using Moq;

namespace JobApplicationTracker.Test.Services;

public class UserServiceTest
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserService _userService;

    public UserServiceTest()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserService(_userRepositoryMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnMappedDtos()
    {
        var users = new List<UserDo>
        {
            new() { Id = 1, Username = "alice" },
            new() { Id = 2, Username = "bob" }
        };

        _userRepositoryMock
            .Setup(r => r.GetAllAsync())
            .ReturnsAsync(users);

        var result = await _userService.GetAllAsync();

        result.Should().HaveCount(2);
        result.Should().ContainSingle(u => u.Id == 1 && u.Username == "alice");
        result.Should().ContainSingle(u => u.Id == 2 && u.Username == "bob");
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnNull_WhenRepositoryReturnsNull()
    {
        _userRepositoryMock
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync((UserDo?)null);

        var result = await _userService.GetByIdAsync(1);

        result.Should().BeNull();
    }

    [Fact]
    public async Task AddAsync_ShouldTrimUsername_AndReturnMappedDto()
    {
        var createDto = new CreateUserDto
        {
            Username = "  alice  "
        };

        _userRepositoryMock
            .Setup(r => r.AddAsync(It.IsAny<UserDo>()))
            .ReturnsAsync((UserDo user) => new UserDo
            {
                Id = 1,
                Username = user.Username
            });

        var result = await _userService.AddAsync(createDto);

        result.Id.Should().Be(1);
        result.Username.Should().Be("alice");

        _userRepositoryMock.Verify(r => r.AddAsync(
            It.Is<UserDo>(u => u.Username == "alice")),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldReturnNull_WhenRepositoryReturnsNull()
    {
        var updateDto = new UpdateUserDto
        {
            Username = "updated"
        };

        _userRepositoryMock
            .Setup(r => r.UpdateAsync(It.IsAny<UserDo>()))
            .ReturnsAsync((UserDo?)null);

        var result = await _userService.UpdateAsync(1, updateDto);

        result.Should().BeNull();
    }

    [Fact]
    public async Task DeleteAsync_ShouldReturnTrue_WhenRepositoryDeletesUser()
    {
        _userRepositoryMock
            .Setup(r => r.DeleteAsync(1))
            .ReturnsAsync(true);

        var result = await _userService.DeleteAsync(1);

        result.Should().BeTrue();
    }
}