using FluentAssertions;
using JobApplicationTracker.Controllers;
using JobApplicationTracker.DTOs;
using JobApplicationTracker.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace JobApplicationTracker.Test.Controllers;

public class UserControllerTests
{
    /*private readonly Mock<IUserService> _userServiceMock;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _userServiceMock = new Mock<IUserService>();
        _controller = new UserController(_userServiceMock.Object);
    }

    [Fact]
    public async Task GetAll_ShouldReturnOk_WithUsers()
    {
        var users = new List<UserDto>
        {
            new() { Id = 1, Username = "alice" },
            new() { Id = 2, Username = "bob" }
        };

        _userServiceMock
            .Setup(s => s.GetAllAsync())
            .ReturnsAsync(users);

        var result = await _controller.GetAll();

        var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
        var returnedUsers = okResult.Value.Should().BeAssignableTo<IEnumerable<UserDto>>().Subject;

        returnedUsers.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetById_ShouldReturnNotFound_WhenUserDoesNotExist()
    {
        _userServiceMock
            .Setup(s => s.GetByIdAsync(1))
            .ReturnsAsync((UserDto?)null);

        var result = await _controller.GetById(1);

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Create_ShouldReturnCreatedAtAction()
    {
        var createDto = new CreateUserDto
        {
            Username = "alice"
        };

        var createdUser = new UserDto
        {
            Id = 1,
            Username = "alice"
        };

        _userServiceMock
            .Setup(s => s.AddAsync(createDto))
            .ReturnsAsync(createdUser);

        var result = await _controller.Create(createDto);

        var createdAt = result.Result.Should().BeOfType<CreatedAtActionResult>().Subject;
        createdAt.ActionName.Should().Be(nameof(UserController.GetById));
        createdAt.Value.Should().Be(createdUser);
    }

    [Fact]
    public async Task Update_ShouldReturnNotFound_WhenServiceReturnsNull()
    {
        var updateDto = new UpdateUserDto
        {
            Username = "updated"
        };

        _userServiceMock
            .Setup(s => s.UpdateAsync(1, updateDto))
            .ReturnsAsync((UserDto?)null);

        var result = await _controller.Update(1, updateDto);

        result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task Delete_ShouldReturnNoContent_WhenDeleteSucceeds()
    {
        _userServiceMock
            .Setup(s => s.DeleteAsync(1))
            .ReturnsAsync(true);

        var result = await _controller.Delete(1);

        result.Should().BeOfType<NoContentResult>();
    }*/
}